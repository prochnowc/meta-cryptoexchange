using System.Text.Json.Serialization;

namespace CryptoExchange.TestData.DataContracts;

public record TestDataSet(
    [property: JsonPropertyName("Id")]
    string ExchangeId,
    AvailableFunds AvailableFunds,
    OrderBook OrderBook);