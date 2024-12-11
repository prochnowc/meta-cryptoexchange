using CryptoExchange.Domain;
using Order = CryptoExchange.Domain.Order;
using OrderKind = CryptoExchange.Domain.OrderKind;
using OrderType = CryptoExchange.Domain.OrderType;

namespace CryptoExchange.TestData;

/// <summary>
/// Maps JSON test data to domain model. We assume that 'Crypto' funds are 'BTC' (Bitcoin).
/// All orders are also assumed to be in 'BTC'.
/// </summary>
internal static class TestDataMapper
{
    public static OrderType MapOrderType(DataContracts.OrderType orderType)
    {
        return orderType switch
        {
            DataContracts.OrderType.Sell => OrderType.Sell,
            DataContracts.OrderType.Buy => OrderType.Buy,
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null),
        };
    }

    public static OrderKind MapOrderKind(DataContracts.OrderKind orderKind)
    {
        return orderKind switch
        {
            DataContracts.OrderKind.Limit => OrderKind.Limit,
            _ => throw new ArgumentOutOfRangeException(nameof(orderKind), orderKind, null),
        };
    }

    public static Order MapOrder(DataContracts.Order order)
    {
        return new Order
        {
            Id = order.Id,
            Time = order.Time,
            Type = MapOrderType(order.Type),
            Kind = MapOrderKind(order.Kind),
            Amount = order.Amount,
            Price = order.Price,
        };
    }

    public static Exchange MapExchange(DataContracts.TestDataSet dataSet)
    {
        IEnumerable<DataContracts.Order> orders =
            dataSet.OrderBook.Asks.Select(ao => ao.Order)
                   .Concat(dataSet.OrderBook.Bids.Select(bo => bo.Order));

        var orderBook = new OrderBook(
            KnownSymbols.BTC,
            orders.Select(MapOrder));

        return new Exchange(dataSet.ExchangeId, new[] { orderBook });
    }

    public static IEnumerable<AccountBalance> MapAccountBalances(DataContracts.TestDataSet dataSet)
    {
        yield return new AccountBalance
        {
            Symbol = KnownSymbols.EUR,
            ExchangeId = dataSet.ExchangeId,
            Amount = dataSet.AvailableFunds.Euro,
        };

        yield return new AccountBalance
        {
            Symbol = KnownSymbols.BTC,
            ExchangeId = dataSet.ExchangeId,
            Amount = dataSet.AvailableFunds.Crypto,
        };
    }
}