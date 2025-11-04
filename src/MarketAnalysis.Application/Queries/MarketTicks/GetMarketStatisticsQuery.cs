namespace MarketAnalysis.Application.Queries.MarketTicks;

using MediatR;
using MarketAnalysis.Application.DTOs;

/// <summary>
/// Query to get market statistics
/// </summary>
public class GetMarketStatisticsQuery : IRequest<MarketStatisticsDto>
{
}
