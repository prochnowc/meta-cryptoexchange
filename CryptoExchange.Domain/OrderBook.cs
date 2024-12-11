namespace CryptoExchange.Domain;

/// <summary>
/// Represents the order book of an exchange.
/// </summary>
public class OrderBook
{
    private readonly string _symbol;
    private readonly List<Order> _orders;

    public string Symbol => _symbol;

    public IReadOnlyCollection<Order> Orders => _orders;

    public OrderBook(string symbol, IEnumerable<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(symbol);
        ArgumentNullException.ThrowIfNull(orders);

        _symbol = symbol;
        _orders = orders.ToList();
    }

    public IEnumerable<Order> GetBestPriceOrders(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Buy => _orders.Where(o => o.Type == OrderType.Sell)
                                    .OrderBy(o => o.Price),

            OrderType.Sell => _orders.Where(o => o.Type == OrderType.Buy)
                                     .OrderByDescending(o => o.Price),

            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null),
        };
    }
}