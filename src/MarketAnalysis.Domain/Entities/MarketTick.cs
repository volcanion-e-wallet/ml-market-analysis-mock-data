namespace MarketAnalysis.Domain.Entities;

using MarketAnalysis.Domain.Common;
using MarketAnalysis.Domain.ValueObjects;

/// <summary>
/// Entity representing real-time market tick data
/// </summary>
public class MarketTick : Entity
{
    public StockSymbol Symbol { get; private set; }
    public Price Price { get; private set; }
    public Volume Volume { get; private set; }
    public decimal Change { get; private set; }
    public decimal ChangePercent { get; private set; }
    public Price High { get; private set; }
    public Price Low { get; private set; }
    public Price Open { get; private set; }
    public Price PreviousClose { get; private set; }
    public DateTime Timestamp { get; private set; }

    private MarketTick() 
    { 
        Symbol = null!;
        Price = null!;
        Volume = null!;
        High = null!;
        Low = null!;
        Open = null!;
        PreviousClose = null!;
    }

    private MarketTick(
        StockSymbol symbol,
        Price price,
        Volume volume,
        Price high,
        Price low,
        Price open,
        Price previousClose,
        DateTime timestamp) : base()
    {
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Volume = volume ?? throw new ArgumentNullException(nameof(volume));
        High = high ?? throw new ArgumentNullException(nameof(high));
        Low = low ?? throw new ArgumentNullException(nameof(low));
        Open = open ?? throw new ArgumentNullException(nameof(open));
        PreviousClose = previousClose ?? throw new ArgumentNullException(nameof(previousClose));
        Timestamp = timestamp;

        CalculateChange();
    }

    public static MarketTick Create(
        string symbol,
        decimal price,
        long volume,
        decimal high,
        decimal low,
        decimal open,
        decimal previousClose,
        DateTime? timestamp = null)
    {
        var stockSymbol = StockSymbol.Create(symbol);
        var priceObj = Price.Create(price);
        var volumeObj = Volume.Create(volume);
        var highObj = Price.Create(high);
        var lowObj = Price.Create(low);
        var openObj = Price.Create(open);
        var previousCloseObj = Price.Create(previousClose);
        var timestampValue = timestamp ?? DateTime.UtcNow;

        return new MarketTick(
            stockSymbol,
            priceObj,
            volumeObj,
            highObj,
            lowObj,
            openObj,
            previousCloseObj,
            timestampValue);
    }

    private void CalculateChange()
    {
        Change = Price.Value - PreviousClose.Value;
        ChangePercent = PreviousClose.Value != 0
            ? (Change / PreviousClose.Value) * 100
            : 0;
    }
}
