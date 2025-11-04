namespace MarketAnalysis.Application.Queries.Stocks;

using MediatR;
using MarketAnalysis.Application.DTOs;

/// <summary>
/// Query to get stock by symbol
/// </summary>
public class GetStockBySymbolQuery : IRequest<StockDto?>
{
    public string Symbol { get; set; } = string.Empty;

    public GetStockBySymbolQuery(string symbol)
    {
        Symbol = symbol;
    }
}
