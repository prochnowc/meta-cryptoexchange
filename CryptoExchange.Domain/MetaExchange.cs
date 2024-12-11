using System.Runtime.CompilerServices;
using CryptoExchange.Domain.Repositories;

namespace CryptoExchange.Domain;

/// <summary>
/// Represents a container for all known exchanges.
/// </summary>
public class MetaExchange
{
    private readonly IExchangeRepository _exchangeRepository;

    public MetaExchange(IExchangeRepository exchangeRepository)
    {
        ArgumentNullException.ThrowIfNull(exchangeRepository);
        _exchangeRepository = exchangeRepository;
    }

    public async IAsyncEnumerable<(string ExchangeId, Order Order)> GetBestPriceOrdersAsync(
        string symbol,
        OrderType orderType,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        // fetch all know exchanges
        List<string> exchangeIds = await _exchangeRepository.GetExchangeIdsAsync(cancellationToken)
                                                            .ToListAsync(cancellationToken);

        // get best price orders for all known exchanges
        var ordersByExchange =
            exchangeIds.ToAsyncEnumerable()
                       .SelectAwaitWithCancellation(
                           async (id, ct) => await _exchangeRepository.GetExchangeAsync(id, ct))
                       .SelectMany(
                           e =>
                               e.GetBestPriceOrders(symbol, orderType)
                                .ToAsyncEnumerable(),
                           (e, o) => new
                           {
                               e.Id,
                               Order = o,
                           });

        // apply price ordering depending on order type
        ordersByExchange = orderType switch
        {
            OrderType.Buy => ordersByExchange
                             .Where(o => o.Order.Type == OrderType.Sell)
                             .OrderBy(o => o.Order.Price),

            OrderType.Sell => ordersByExchange
                              .Where(o => o.Order.Type == OrderType.Buy)
                              .OrderByDescending(o => o.Order.Price),

            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null),
        };

        await foreach (var orderByExchange in ordersByExchange.WithCancellation(cancellationToken))
        {
            yield return (orderByExchange.Id, orderByExchange.Order);
        }
    }
}