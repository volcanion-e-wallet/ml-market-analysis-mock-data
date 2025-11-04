namespace MarketAnalysis.SimulationJob.Services;

using MarketAnalysis.SimulationJob.Models;

public interface IMarketSimulator
{
    MarketTickRequest GenerateTick(StockDto stock, decimal currentPrice);
}

public class MarketSimulator : IMarketSimulator
{
    private readonly Random _random = new();
    private readonly double _minPriceChange;
    private readonly double _maxPriceChange;
    private readonly int _minVolume;
    private readonly int _maxVolume;

    public MarketSimulator()
    {
        _minPriceChange = -0.02; // -2%
        _maxPriceChange = 0.02;  // +2%
        _minVolume = 100000;
        _maxVolume = 10000000;
    }

    public MarketTickRequest GenerateTick(StockDto stock, decimal currentPrice)
    {
        // Simulate price movement
        var priceChange = (decimal)(_random.NextDouble() * (_maxPriceChange - _minPriceChange) + _minPriceChange);
        var newPrice = currentPrice * (1 + priceChange);
        newPrice = Math.Round(newPrice, 2);

        // Simulate daily range
        var highMultiplier = 1 + (decimal)(_random.NextDouble() * 0.01);
        var lowMultiplier = 1 - (decimal)(_random.NextDouble() * 0.01);
        
        var high = Math.Round(newPrice * highMultiplier, 2);
        var low = Math.Round(newPrice * lowMultiplier, 2);
        var open = currentPrice;
        var previousClose = currentPrice;

        // Generate random volume
        var volume = _random.Next(_minVolume, _maxVolume);

        return new MarketTickRequest
        {
            Symbol = stock.Symbol,
            Price = newPrice,
            Volume = volume,
            High = high,
            Low = low,
            Open = open,
            PreviousClose = previousClose,
            Timestamp = DateTime.UtcNow
        };
    }
}
