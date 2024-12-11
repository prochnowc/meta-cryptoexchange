namespace CryptoExchange.Domain;

public record Order
{
    public Guid Id { get; init; }

    public DateTimeOffset Time { get; init; }

    public OrderType Type { get; init; }

    public OrderKind Kind { get; init; }

    public decimal Amount { get; init; }

    public decimal Price { get; init; }
}