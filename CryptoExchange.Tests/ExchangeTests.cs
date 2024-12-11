using CryptoExchange.Domain;
using FluentAssertions;

namespace CryptoExchange.Tests;

public class ExchangeTests
{
    public static readonly Order OrderBook1Order = new()
        { Id = Guid.NewGuid() };

    public static readonly OrderBook OrderBook1 = new(
        KnownSymbols.BTC,
        new[] { OrderBook1Order });

    public static readonly Order OrderBook2Order = new()
        { Id = Guid.NewGuid() };

    public static readonly OrderBook OrderBook2 = new(
        KnownSymbols.EUR,
        new[] { OrderBook2Order });

    public static readonly Dictionary<string, OrderBook> OrderBookBySymbol = new()
    {
        { OrderBook1.Symbol, OrderBook1 },
        { OrderBook2.Symbol, OrderBook2 },
    };

    [Theory]
    [InlineData(KnownSymbols.EUR)]
    [InlineData(KnownSymbols.BTC)]
    public void GetBestPriceOrdersOnlyReturnsOrdersFromOrderBookWithSymbol(string symbol)
    {
        var exchange = new Exchange(
            "exchange-01",
            new[] { OrderBook1, OrderBook2 });

        Order[] orders = exchange.GetBestPriceOrders(symbol, OrderType.Sell)
                                 .ToArray();

        orders.Should()
              .BeEquivalentTo(OrderBookBySymbol[symbol].Orders);
    }
}