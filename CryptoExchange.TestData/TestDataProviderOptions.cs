namespace CryptoExchange.TestData;

public class TestDataProviderOptions
{
    public required string BasePath { get; set; }

    public List<string> ExchangeIds { get; } = [
        "exchange-01",
        "exchange-02",
        "exchange-03",
        "exchange-04",
        "exchange-05",
        "exchange-06",
        "exchange-07",
        "exchange-08",
        "exchange-09",
        "exchange-10",
    ];
}