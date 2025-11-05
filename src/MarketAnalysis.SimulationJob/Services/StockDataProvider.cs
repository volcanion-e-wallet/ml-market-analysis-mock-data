namespace MarketAnalysis.SimulationJob.Services;

using MarketAnalysis.SimulationJob.Models;

public interface IStockDataProvider
{
    List<CreateStockRequest> GetSampleStocks();
}

public class StockDataProvider : IStockDataProvider
{
    public List<CreateStockRequest> GetSampleStocks()
    {
        return
        [
            new() { Symbol = "AAPL", Name = "Apple Inc.", Exchange = "NASDAQ", Sector = "Technology", Industry = "Consumer Electronics", InitialPrice = 175.50m },
            new() { Symbol = "GOOGL", Name = "Alphabet Inc.", Exchange = "NASDAQ", Sector = "Technology", Industry = "Internet Services", InitialPrice = 140.25m },
            new() { Symbol = "MSFT", Name = "Microsoft Corp.", Exchange = "NASDAQ", Sector = "Technology", Industry = "Software", InitialPrice = 380.75m },
            new() { Symbol = "AMZN", Name = "Amazon.com Inc.", Exchange = "NASDAQ", Sector = "Consumer Cyclical", Industry = "Internet Retail", InitialPrice = 145.80m },
            new() { Symbol = "TSLA", Name = "Tesla Inc.", Exchange = "NASDAQ", Sector = "Consumer Cyclical", Industry = "Auto Manufacturers", InitialPrice = 245.30m },
            new() { Symbol = "META", Name = "Meta Platforms Inc.", Exchange = "NASDAQ", Sector = "Technology", Industry = "Social Media", InitialPrice = 485.60m },
            new() { Symbol = "NVDA", Name = "NVIDIA Corp.", Exchange = "NASDAQ", Sector = "Technology", Industry = "Semiconductors", InitialPrice = 495.25m },
            new() { Symbol = "JPM", Name = "JPMorgan Chase & Co.", Exchange = "NYSE", Sector = "Financial", Industry = "Banks", InitialPrice = 185.40m },
            new() { Symbol = "V", Name = "Visa Inc.", Exchange = "NYSE", Sector = "Financial", Industry = "Credit Services", InitialPrice = 265.90m },
            new() { Symbol = "WMT", Name = "Walmart Inc.", Exchange = "NYSE", Sector = "Consumer Defensive", Industry = "Retail", InitialPrice = 165.75m }
        ];
    }
}
