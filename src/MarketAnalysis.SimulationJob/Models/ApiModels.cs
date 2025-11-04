namespace MarketAnalysis.SimulationJob.Models;

public class StockDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal CurrentPrice { get; set; }
    public bool IsActive { get; set; }
}

public class CreateStockRequest
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal InitialPrice { get; set; }
}

public class MarketTickRequest
{
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long Volume { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Open { get; set; }
    public decimal PreviousClose { get; set; }
    public DateTime? Timestamp { get; set; }
}

public class BatchMarketTicksRequest
{
    public List<MarketTickRequest> Ticks { get; set; } = new();
}
