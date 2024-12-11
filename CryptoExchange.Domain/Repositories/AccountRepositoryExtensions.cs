namespace CryptoExchange.Domain.Repositories;

public static class AccountRepositoryExtensions
{
    public static async Task<Account> GetAccountAsync(
        this IAccountRepository repository,
        string id,
        CancellationToken cancellationToken = default)
    {
        Account? result = await repository.FindAccountAsync(id, cancellationToken);
        if (result == null)
        {
            throw new ApplicationException($"Account with id '{id}' not found");
        }

        return result;
    }
}