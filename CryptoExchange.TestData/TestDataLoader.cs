using System.Text.Json;
using CryptoExchange.TestData.DataContracts;

namespace CryptoExchange.TestData;

internal class TestDataLoader
{
    private readonly string _basePath;

    public TestDataLoader(string basePath)
    {
        ArgumentNullException.ThrowIfNull(basePath);
        _basePath = basePath;
    }

    public async Task<TestDataSet?> LoadAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        await using Stream stream = new FileStream(
            Path.Combine(_basePath, $"{id}.json"),
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            4096,
            FileOptions.Asynchronous);

        return (TestDataSet?)await JsonSerializer.DeserializeAsync(
            stream,
            typeof(TestDataSet),
            TestDataJsonSerializerContext.Default,
            cancellationToken);
    }
}