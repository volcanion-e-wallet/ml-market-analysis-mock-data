namespace MarketAnalysis.Application.Queries.Stocks;

using MediatR;
using MarketAnalysis.Application.DTOs;

/// <summary>
/// Query to get all stocks
/// </summary>
public class GetAllStocksQuery : IRequest<IEnumerable<StockDto>>
{
}
