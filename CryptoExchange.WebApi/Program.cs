using CryptoExchange.Domain;
using CryptoExchange.Domain.Repositories;
using CryptoExchange.TestData;
using CryptoExchange.WebApi;
using CryptoExchange.WebApi.Binding;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

// register Json serializer options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// register repositories and the meta exchange
builder.Services.AddSingleton<TestDataProvider>(
    _ => new TestDataProvider(
        new TestDataProviderOptions
        {
            BasePath = Path.Combine(AppContext.BaseDirectory, "Data"),
        }));

builder.Services.AddScoped<IAccountRepository, TestDataAccountRepository>();
builder.Services.AddScoped<IExchangeRepository, TestDataExchangeRepository>();
builder.Services.AddScoped<MetaExchange>();

WebApplication app = builder.Build();

RouteGroupBuilder bestPriceApi = app.MapGroup("/best-price");
bestPriceApi.MapGet(
    "/",
    async (
        [FromQuery] EnumBinding<OrderType> orderType,
        [FromQuery] decimal amount,
        [FromServices] MetaExchange exchange,
        [FromServices] IAccountRepository repository,
        CancellationToken cancellationToken) =>
    {
        Account account = await repository.GetAccountAsync("0", cancellationToken);
        BestPriceOrderGenerator bestPriceOrderGenerator = new(exchange, account);
        BestPriceOrder[] result =
            await bestPriceOrderGenerator.GenerateBestPriceOrdersAsync(
                                             KnownSymbols.BTC,
                                             orderType,
                                             amount,
                                             cancellationToken)
                                         .ToArrayAsync(cancellationToken);
        return Results.Ok(result);
    });

app.Run();