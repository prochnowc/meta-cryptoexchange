using CryptoExchange.TestData.DataContracts;

namespace CryptoExchange.TestData;

public class TestDataProvider
{
    private readonly TestDataProviderOptions _options;
    private List<TestDataSet>? _dataSets;

    public TestDataProvider(TestDataProviderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    public async Task<IEnumerable<TestDataSet>> GetDataSetsAsync(CancellationToken cancellationToken = default)
    {
        if (_dataSets != null)
            return _dataSets;

        TestDataLoader loader = new(_options.BasePath);
        List<TestDataSet> dataSets = new();
        foreach (string exchangeId in _options.ExchangeIds)
        {
            TestDataSet? testDataSet = await loader.LoadAsync(exchangeId, cancellationToken);
            if (testDataSet != null)
            {
                dataSets.Add(testDataSet);
            }
        }

        return _dataSets = dataSets;
    }
}