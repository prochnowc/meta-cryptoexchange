namespace CryptoExchange.Domain;

/// <summary>
/// Represents an Exchange.
/// </summary>
public class Exchange
{
    private readonly string _id;
    private readonly Dictionary<string, OrderBook> _orderBooks;

    public string Id => _id;

    public IReadOnlyCollection<OrderBook> OrderBooks => _orderBooks.Values;

    public Exchange(string id, IEnumerable<OrderBook> orderBooks)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(orderBooks);

        _id = id;
        _orderBooks = orderBooks.ToDictionary(ob => ob.Symbol, ob => ob);
    }

    public IEnumerable<Order> GetBestPriceOrders(string symbol, OrderType orderType)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        OrderBook? orderBook = _orderBooks.GetValueOrDefault(symbol);
        return orderBook is null ? [] : orderBook.GetBestPriceOrders(orderType);
    }
}