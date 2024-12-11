namespace CryptoExchange.Domain.Repositories;

public static class ExchangeRepositoryExtensions
{
    public static async Task<Exchange> GetExchangeAsync(
        this IExchangeRepository repository,
        string id,
        CancellationToken cancellationToken = default)
    {
        Exchange? result = await repository.FindExchangeAsync(id, cancellationToken);
        if (result == null)
        {
            throw new ApplicationException($"Exchange with id '{id}' not found");
        }

        return result;
    }
}