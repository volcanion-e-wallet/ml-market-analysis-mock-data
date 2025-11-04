namespace MarketAnalysis.Domain.Entities;

using MarketAnalysis.Domain.Common;
using MarketAnalysis.Domain.ValueObjects;

/// <summary>
/// Entity representing historical trading data
/// </summary>
public class TradingHistory : Entity
{
    public StockSymbol Symbol { get; private set; }
    public DateTime Date { get; private set; }
    public Price Open { get; private set; }
    public Price High { get; private set; }
    public Price Low { get; private set; }
    public Price Close { get; private set; }
    public Volume Volume { get; private set; }
    public Price AdjustedClose { get; private set; }

    private TradingHistory() 
    { 
        Symbol = null!;
        Open = null!;
        High = null!;
        Low = null!;
        Close = null!;
        Volume = null!;
        AdjustedClose = null!;
    }

    private TradingHistory(
        StockSymbol symbol,
        DateTime date,
        Price open,
        Price high,
        Price low,
        Price close,
        Volume volume,
        Price adjustedClose) : base()
    {
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        Date = date;
        Open = open ?? throw new ArgumentNullException(nameof(open));
        High = high ?? throw new ArgumentNullException(nameof(high));
        Low = low ?? throw new ArgumentNullException(nameof(low));
        Close = close ?? throw new ArgumentNullException(nameof(close));
        Volume = volume ?? throw new ArgumentNullException(nameof(volume));
        AdjustedClose = adjustedClose ?? throw new ArgumentNullException(nameof(adjustedClose));
    }

    public static TradingHistory Create(
        string symbol,
        DateTime date,
        decimal open,
        decimal high,
        decimal low,
        decimal close,
        long volume,
        decimal adjustedClose)
    {
        var stockSymbol = StockSymbol.Create(symbol);
        var openPrice = Price.Create(open);
        var highPrice = Price.Create(high);
        var lowPrice = Price.Create(low);
        var closePrice = Price.Create(close);
        var volumeObj = Volume.Create(volume);
        var adjustedClosePrice = Price.Create(adjustedClose);

        return new TradingHistory(
            stockSymbol,
            date,
            openPrice,
            highPrice,
            lowPrice,
            closePrice,
            volumeObj,
            adjustedClosePrice);
    }
}
