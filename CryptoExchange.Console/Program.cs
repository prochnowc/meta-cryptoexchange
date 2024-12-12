using CryptoExchange.Domain;
using CryptoExchange.Domain.Repositories;
using CryptoExchange.TestData;
using Spectre.Console;

var testDataProvider = new TestDataProvider(
    new TestDataProviderOptions
    {
        BasePath = Path.Combine(AppContext.BaseDirectory, "Data"),
    });

TestDataAccountRepository accountRepository = new(testDataProvider);
TestDataExchangeRepository exchangeRepository = new(testDataProvider);

MetaExchange exchange = new(exchangeRepository);
Account account = await accountRepository.GetAccountAsync("0");

BestPriceOrderGenerator bestPriceOrderGenerator = new(exchange, account);

AnsiConsole.WriteLine("Press Ctrl+C to exit");

while (true)
{
    OrderType orderType = AnsiConsole.Prompt(
        new SelectionPrompt<OrderType>()
            .Title("Type of order?")
            .AddChoices(OrderType.Buy, OrderType.Sell));

    AnsiConsole.WriteLine($"Type of order? {orderType}");
    decimal amount = AnsiConsole.Ask<decimal>("Amount?");

    IAsyncEnumerable<BestPriceOrder> bestPriceOrders =
        bestPriceOrderGenerator.GenerateBestPriceOrdersAsync(
            KnownSymbols.BTC,
            orderType,
            amount);

    decimal totalAmount = decimal.Zero;
    await foreach (BestPriceOrder order in bestPriceOrders)
    {
        AnsiConsole.WriteLine(
            $"Matched Order with Id '{order.MatchedOrder.Id}' at Exchange '{order.ExchangeId}'"
            + $" Amount: {order.MatchedOrder.Amount} Price: {order.MatchedOrder.Price}, Matched amount: {order.Amount} Partial Order: {order.IsPartial}");

        totalAmount += order.Amount;
    }

    AnsiConsole.WriteLine($"Total Order Amount: {totalAmount}");
    AnsiConsole.WriteLine();
}