using CryptoExchange.Domain;
using CryptoExchange.Domain.Repositories;
using CryptoExchange.TestData.DataContracts;

namespace CryptoExchange.TestData;

public class TestDataAccountRepository : IAccountRepository
{
    private readonly TestDataProvider _dataProvider;

    public TestDataAccountRepository(TestDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);
        _dataProvider = dataProvider;
    }

    public async Task<Account?> FindAccountAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        List<AccountBalance> accountBalances = new();
        foreach (TestDataSet dataSet in await _dataProvider.GetDataSetsAsync(cancellationToken))
        {
            accountBalances.AddRange(TestDataMapper.MapAccountBalances(dataSet));
        }

        return new Account(id, accountBalances);
    }
}