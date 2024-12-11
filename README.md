Meta CryptoExchange
===================

In this repository you will find the Meta CryptoExchange test application
which finds the best price orders from a number of exchanges and outputs
the orders to execute.

The project is fully AOT compatible.

Some assumptions were made:
- All OrderBooks represent Bitcoin orders
- Orders can be executed partially
- Available Crypto funds are in Bitcoin


The repository contains 5 projects:

- CryptoExchange.Console: The console application which can be used to output
  best price orders.
- CryptoExchange.WebApi: The Web API project which can be used to output
  best price orders.
- CryptoExchange.Domain: The meta crypto exchange domain logic.
- CryptoExchange.TestData: Infrastructure to load test data and make them
  available to the domain logic.
- CryptoExchange.Tests: Some unit tests.

Pre-requisites
---

- .NET 8 SDK
- Docker (if you want to build an run the Web API in a container)

Build and run the Console tool
---

```sh
dotnet run --project ./CryptoExchange.Console/CryptoExchange.Console.csproj
```

Follow the instructions on the console. To exit the program press Ctrl+C.

Build and run the Web API
---

```sh
dotnet run --project ./CryptoExchange.WebApi/CryptoExchange.WebApi.csproj
```

By default, the API is available under port 5097 and exposes a single endpoint
to query the best price order:

`http://localhost:5097/best-price?orderType={oderType}&amount={amount}`

orderType can be one of `Buy` or `Sell`. The amount is the amount to buy or sell.

Run the Web API in a Docker container
---

To build the Web API as a Docker container issue the following command:

```sh
docker build --tag cryptoexchange:dev --file ./CryptoExchange.WebApi/Dockerfile .
```

After build is successful you can run the Image with the following command:

```sh
docker run --rm -p 8080:8080 cryptoexchange:dev
```

Now you can access the API endpoint using your favorite HTTP tool:

`http://localhost:8080/best-price?orderType=Sell&amount=1`
