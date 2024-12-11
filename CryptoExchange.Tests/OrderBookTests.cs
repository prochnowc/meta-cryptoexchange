using CryptoExchange.Domain;
using FluentAssertions;

namespace CryptoExchange.Tests;

public class OrderBookTests
{
    public static readonly Order BuyOrder1 = new()
    {
        Id = Guid.NewGuid(),
        Type = OrderType.Buy,
        Price = 50,
    };

    public static readonly Order BuyOrder2 = new()
    {
        Id = Guid.NewGuid(),
        Type = OrderType.Buy,
        Price = 100,
    };

    public static readonly Order BuyOrder3 = new()
    {
        Id = Guid.NewGuid(),
        Type = OrderType.Buy,
        Price = 200,
    };

    public static readonly Order[] BuyOrders = [BuyOrder1, BuyOrder2, BuyOrder3];

    public static readonly Order SellOrder1 = new()
    {
        Id = Guid.NewGuid(),
        Type = OrderType.Sell,
        Price = 200,
    };

    public static readonly Order SellOrder2 = new()
    {
        Id = Guid.NewGuid(),
        Type = OrderType.Sell,
        Price = 100,
    };

    public static readonly Order SellOrder3 = new()
    {
        Id = Guid.NewGuid(),
        Type = OrderType.Sell,
        Price = 50,
    };

    public static readonly Order[] SellOrders = [SellOrder1, SellOrder2, SellOrder3];

    [Fact]
    public void GetBestPriceSellOrdersReturnsOnlyBuyOrders()
    {
        var orderBook = new OrderBook(KnownSymbols.BTC, BuyOrders.Concat(SellOrders));
        Order[] result = orderBook.GetBestPriceOrders(OrderType.Sell)
                                  .ToArray();

        result.Should()
              .OnlyContain(o => o.Type == OrderType.Buy);
    }

    [Fact]
    public void GetBestPriceBuyOrdersReturnsOnlySellOrders()
    {
        var orderBook = new OrderBook(KnownSymbols.BTC, BuyOrders.Concat(SellOrders));
        Order[] result = orderBook.GetBestPriceOrders(OrderType.Buy)
                                  .ToArray();

        result.Should()
              .OnlyContain(o => o.Type == OrderType.Sell);
    }

    [Fact]
    public void GetBestPriceSellOrdersReturnsOrdersWithDescendingPrice()
    {
        var orderBook = new OrderBook(KnownSymbols.BTC, BuyOrders);
        Order[] result = orderBook.GetBestPriceOrders(OrderType.Sell)
                                  .ToArray();

        result.Should()
              .HaveCount(3);

        result[0]
            .Id.Should()
            .Be(BuyOrder3.Id);

        result[1]
            .Id.Should()
            .Be(BuyOrder2.Id);

        result[2]
            .Id.Should()
            .Be(BuyOrder1.Id);
    }

    [Fact]
    public void GetBestPriceBuyOrdersReturnsOrdersWithAscendingPrice()
    {
        var orderBook = new OrderBook(KnownSymbols.BTC, SellOrders);
        Order[] result = orderBook.GetBestPriceOrders(OrderType.Buy)
                                  .ToArray();

        result.Should()
              .HaveCount(3);

        result[0]
            .Id.Should()
            .Be(SellOrder3.Id);

        result[1]
            .Id.Should()
            .Be(SellOrder2.Id);

        result[2]
            .Id.Should()
            .Be(SellOrder1.Id);
    }
}