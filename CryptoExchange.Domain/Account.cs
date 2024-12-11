namespace CryptoExchange.Domain;

/// <summary>
/// Represents a trader account.
/// </summary>
public class Account
{
    private readonly string _id;
    private readonly List<AccountBalance> _balances;

    public string Id => _id;

    public IReadOnlyCollection<AccountBalance> Balances => _balances;

    public Account(string id, IEnumerable<AccountBalance> balances)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(balances);

        _id = id;
        _balances = balances.ToList();
    }

    public decimal GetAccountBalance(string exchangeId, string symbol)
    {
        ArgumentNullException.ThrowIfNull(exchangeId);
        ArgumentNullException.ThrowIfNull(symbol);

        AccountBalance? accountBalance = _balances
            .FirstOrDefault(b => b.ExchangeId == exchangeId && b.Symbol == symbol);

        return accountBalance?.Amount ?? decimal.Zero;
    }
}