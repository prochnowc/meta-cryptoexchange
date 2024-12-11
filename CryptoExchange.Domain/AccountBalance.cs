namespace CryptoExchange.Domain;

/// <summary>
/// Represents the account balance for a given exchange and symbol.
/// </summary>
public record AccountBalance
{
    public required string ExchangeId { get; init; }

    public required string Symbol { get; init; }

    public decimal Amount { get; init; }
}