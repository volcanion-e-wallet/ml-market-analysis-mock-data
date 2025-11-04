namespace MarketAnalysis.Application.Commands.Stocks;

using MediatR;

/// <summary>
/// Command to update stock price
/// </summary>
public class UpdateStockPriceCommand : IRequest<Unit>
{
    public string Symbol { get; set; } = string.Empty;
    public decimal NewPrice { get; set; }
}
