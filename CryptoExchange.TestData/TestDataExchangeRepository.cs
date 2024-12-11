using System.Runtime.CompilerServices;
using CryptoExchange.Domain;
using CryptoExchange.Domain.Repositories;
using CryptoExchange.TestData.DataContracts;

namespace CryptoExchange.TestData;

public class TestDataExchangeRepository : IExchangeRepository
{
    private readonly TestDataProvider _dataProvider;

    public TestDataExchangeRepository(TestDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);
        _dataProvider = dataProvider;
    }

    public async IAsyncEnumerable<string> GetExchangeIdsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IEnumerable<TestDataSet> dataSets = await _dataProvider.GetDataSetsAsync(cancellationToken);
        foreach (TestDataSet dataSet in dataSets)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return dataSet.ExchangeId;
        }
    }

    public async Task<Exchange?> FindExchangeAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        IEnumerable<TestDataSet> dataSets = await _dataProvider.GetDataSetsAsync(cancellationToken);

        TestDataSet? result = dataSets.FirstOrDefault(d => d.ExchangeId == id);
        return result != null
            ? TestDataMapper.MapExchange(result)
            : null;
    }
}