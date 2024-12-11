namespace CryptoExchange.Domain;

/// <summary>
/// Represents a best price order for a given exchange.
/// </summary>
public record BestPriceOrder
{
    /// <summary>
    /// Gets the ID of the Exchange where the order should be executed.
    /// </summary>
    public required string ExchangeId { get; init; }

    /// <summary>
    /// Gets the order that should be executed.
    /// </summary>
    public required Order MatchedOrder { get; init; }

    /// <summary>
    /// Gets the amount to execute for the matched order.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Gets the type of the order to generate.
    /// </summary>
    public OrderType Type { get; init; }

    /// <summary>
    /// Gets a value indicating whether the order is partially matched.
    /// </summary>
    public bool IsPartial { get; init; }
}