namespace MarketAnalysis.Application.Commands.MarketTicks;

using MediatR;

/// <summary>
/// Command to create a new market tick
/// </summary>
public class CreateMarketTickCommand : IRequest<Guid>
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
