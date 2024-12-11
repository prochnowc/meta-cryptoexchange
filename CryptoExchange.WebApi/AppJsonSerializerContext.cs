using System.Text.Json.Serialization;
using CryptoExchange.Domain;

namespace CryptoExchange.WebApi;

[JsonSerializable(typeof(BestPriceOrder[]))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true)]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}