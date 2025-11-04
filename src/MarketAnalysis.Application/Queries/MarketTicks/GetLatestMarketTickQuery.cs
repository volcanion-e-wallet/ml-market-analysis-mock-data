namespace MarketAnalysis.Application.Queries.MarketTicks;

using MediatR;
using MarketAnalysis.Application.DTOs;

/// <summary>
/// Query to get latest market tick by symbol
/// </summary>
public class GetLatestMarketTickQuery : IRequest<MarketTickDto?>
{
    public string Symbol { get; set; } = string.Empty;

    public GetLatestMarketTickQuery(string symbol)
    {
        Symbol = symbol;
    }
}
