namespace CryptoExchange.Domain.Repositories;

public interface IAccountRepository
{
    Task<Account?> FindAccountAsync(string id, CancellationToken cancellationToken = default);
}