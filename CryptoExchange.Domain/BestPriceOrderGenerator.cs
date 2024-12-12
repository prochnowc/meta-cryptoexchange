using System.Runtime.CompilerServices;

namespace CryptoExchange.Domain;

/// <summary>
/// Generates best price orders for all known exchanges for a given account.
/// </summary>
/// <remarks>
/// Assumes that partial order execution is allowed.
/// </remarks>
public class BestPriceOrderGenerator
{
    private readonly MetaExchange _exchange;
    private readonly Account _account;

    public BestPriceOrderGenerator(MetaExchange exchange, Account account)
    {
        ArgumentNullException.ThrowIfNull(exchange);
        ArgumentNullException.ThrowIfNull(account);

        _exchange = exchange;
        _account = account;
    }

    public async IAsyncEnumerable<BestPriceOrder> GenerateBestPriceOrdersAsync(
        string symbol,
        OrderType orderType,
        decimal amount,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        string balanceSymbol = orderType switch
        {
            OrderType.Buy => KnownSymbols.EUR,
            OrderType.Sell => KnownSymbols.BTC,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null),
        };

        Dictionary<string, decimal> balanceByExchange = new();
        decimal remainingAmount = amount;

        IAsyncEnumerable<(string ExchangeId, Order Order)> bestPriceOrdersByExchange =
            _exchange.GetBestPriceOrdersAsync(
                symbol,
                orderType,
                cancellationToken);

        await foreach ((string exchangeId, Order order) in bestPriceOrdersByExchange)
        {
            // retrieve current account balance based on exchange
            if (!balanceByExchange.TryGetValue(exchangeId, out decimal accountBalance))
            {
                // no balance found, retrieve it from the account
                accountBalance = _account.GetAccountBalance(exchangeId, balanceSymbol);
                balanceByExchange.Add(exchangeId, accountBalance);
            }

            decimal amountTradeable;
            if (orderType == OrderType.Buy)
            {
                // Calculate maximum Crypto that can be bought with available balance
                decimal maxAmountByBalance = accountBalance / order.Price;
                amountTradeable = Math.Min(order.Amount, Math.Min(remainingAmount, maxAmountByBalance));
            }
            else
            {
                // Calculate maximum Crypto that can be sold based on symbol balance
                amountTradeable = Math.Min(order.Amount, Math.Min(remainingAmount, accountBalance));
            }

            if (amountTradeable > 0)
            {
                yield return new BestPriceOrder
                {
                    ExchangeId = exchangeId,
                    MatchedOrder = order,
                    Type = orderType,
                    Amount = amountTradeable,
                    IsPartial = order.Amount != amountTradeable,
                };

                // Deduct traded cost or amount and update balance
                if (orderType == OrderType.Buy)
                {
                    decimal costInEuro = amountTradeable * order.Price;
                    balanceByExchange[exchangeId] -= costInEuro;
                }
                else
                {
                    balanceByExchange[exchangeId] -= amountTradeable;
                }

                remainingAmount -= amountTradeable;
            }

            if (remainingAmount == decimal.Zero)
                break;
        }
    }
}