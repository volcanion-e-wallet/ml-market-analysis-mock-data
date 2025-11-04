namespace MarketAnalysis.Application.Commands.Stocks;

using MediatR;

/// <summary>
/// Command to create a new stock
/// </summary>
public class CreateStockCommand : IRequest<Guid>
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal InitialPrice { get; set; }
}
