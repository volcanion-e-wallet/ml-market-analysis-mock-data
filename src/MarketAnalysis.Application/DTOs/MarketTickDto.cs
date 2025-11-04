namespace MarketAnalysis.Application.DTOs;

/// <summary>
/// Market tick DTO for real-time data
/// </summary>
public class MarketTickDto
{
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public long Volume { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercent { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Open { get; set; }
    public decimal PreviousClose { get; set; }
    public DateTime Timestamp { get; set; }
}
