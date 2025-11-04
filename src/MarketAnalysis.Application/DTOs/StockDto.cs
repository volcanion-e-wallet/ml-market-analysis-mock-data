namespace MarketAnalysis.Application.DTOs;

/// <summary>
/// Stock DTO for data transfer
/// </summary>
public class StockDto
{
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal CurrentPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal MarketCap { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
