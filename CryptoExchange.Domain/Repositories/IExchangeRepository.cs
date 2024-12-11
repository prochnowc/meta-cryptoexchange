namespace CryptoExchange.Domain.Repositories;

public interface IExchangeRepository
{
    IAsyncEnumerable<string> GetExchangeIdsAsync(CancellationToken cancellationToken = default);

    Task<Exchange?> FindExchangeAsync(string id, CancellationToken cancellationToken = default);
}