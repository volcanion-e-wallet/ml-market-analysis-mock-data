namespace MarketAnalysis.Domain.Entities;

using MarketAnalysis.Domain.Common;
using MarketAnalysis.Domain.ValueObjects;

/// <summary>
/// Aggregate root representing a stock
/// </summary>
public class Stock : Entity, IAggregateRoot
{
    public StockSymbol Symbol { get; private set; }
    public string Name { get; private set; }
    public string Exchange { get; private set; }
    public string Sector { get; private set; }
    public string Industry { get; private set; }
    public Price CurrentPrice { get; private set; }
    public decimal MarketCap { get; private set; }
    public bool IsActive { get; private set; }

    private Stock() 
    { 
        Symbol = null!;
        Name = string.Empty;
        Exchange = string.Empty;
        Sector = string.Empty;
        Industry = string.Empty;
        CurrentPrice = null!;
    }

    private Stock(
        StockSymbol symbol,
        string name,
        string exchange,
        string sector,
        string industry,
        Price initialPrice) : base()
    {
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
        Sector = sector ?? throw new ArgumentNullException(nameof(sector));
        Industry = industry ?? throw new ArgumentNullException(nameof(industry));
        CurrentPrice = initialPrice ?? throw new ArgumentNullException(nameof(initialPrice));
        IsActive = true;
        MarketCap = 0;
    }

    public static Stock Create(
        string symbol,
        string name,
        string exchange,
        string sector,
        string industry,
        decimal initialPrice)
    {
        var stockSymbol = StockSymbol.Create(symbol);
        var price = Price.Create(initialPrice);

        return new Stock(stockSymbol, name, exchange, sector, industry, price);
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be positive", nameof(newPrice));

        CurrentPrice = Price.Create(newPrice, CurrentPrice.Currency);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMarketCap(decimal marketCap)
    {
        if (marketCap < 0)
            throw new ArgumentException("Market cap cannot be negative", nameof(marketCap));

        MarketCap = marketCap;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateInfo(string name, string sector, string industry)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Sector = sector ?? throw new ArgumentNullException(nameof(sector));
        Industry = industry ?? throw new ArgumentNullException(nameof(industry));
        UpdatedAt = DateTime.UtcNow;
    }
}
