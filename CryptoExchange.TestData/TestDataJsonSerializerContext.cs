using System.Text.Json.Serialization;
using CryptoExchange.TestData.DataContracts;

namespace CryptoExchange.TestData;

[JsonSerializable(typeof(TestDataSet))]
[JsonSerializable(typeof(decimal))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    UseStringEnumConverter = true)]
internal partial class TestDataJsonSerializerContext : JsonSerializerContext
{
}