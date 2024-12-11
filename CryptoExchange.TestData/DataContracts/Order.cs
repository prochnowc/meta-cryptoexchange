namespace CryptoExchange.TestData.DataContracts;

public record Order(
    Guid Id,
    DateTimeOffset Time,
    OrderType Type,
    OrderKind Kind,
    decimal Amount,
    decimal Price);