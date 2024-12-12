FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt update && apt install -y clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["global.json", "./"]
COPY ["stylecop.json", "./"]
COPY ["cryptoexchange.ruleset", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["CryptoExchange.WebApi/CryptoExchange.WebApi.csproj", "CryptoExchange.WebApi/"]
COPY ["CryptoExchange.Domain/CryptoExchange.Domain.csproj", "CryptoExchange.Domain/"]
COPY ["CryptoExchange.TestData/CryptoExchange.TestData.csproj", "CryptoExchange.TestData/"]
RUN dotnet restore "CryptoExchange.WebApi/CryptoExchange.WebApi.csproj"
COPY . .
WORKDIR "/src/CryptoExchange.WebApi"
RUN dotnet build "CryptoExchange.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CryptoExchange.WebApi.csproj" -r linux-x64 -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./CryptoExchange.WebApi"]
