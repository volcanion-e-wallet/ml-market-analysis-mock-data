namespace MarketAnalysis.Application.DTOs;

/// <summary>
/// Market statistics DTO
/// </summary>
public class MarketStatisticsDto
{
    public int TotalStocks { get; set; }
    public int ActiveStocks { get; set; }
    public decimal TotalMarketCap { get; set; }
    public decimal AveragePrice { get; set; }
    public long TotalVolume { get; set; }
    public int AdvancingStocks { get; set; }
    public int DecliningStocks { get; set; }
    public int UnchangedStocks { get; set; }
    public DateTime GeneratedAt { get; set; }
}
