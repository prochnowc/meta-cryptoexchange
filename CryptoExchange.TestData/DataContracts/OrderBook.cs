namespace CryptoExchange.TestData.DataContracts;

public record OrderBook(
    IReadOnlyCollection<BidOrder> Bids,
    IReadOnlyCollection<AskOrder> Asks);