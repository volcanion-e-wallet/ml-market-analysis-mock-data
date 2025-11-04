namespace MarketAnalysis.Application.Queries.MarketTicks;

using MediatR;
using MarketAnalysis.Application.DTOs;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for GetMarketStatisticsQuery
/// </summary>
public class GetMarketStatisticsQueryHandler : IRequestHandler<GetMarketStatisticsQuery, MarketStatisticsDto>
{
    private readonly IStockQueryRepository _stockRepository;
    private readonly IMarketTickQueryRepository _tickRepository;
    private readonly ILogger<GetMarketStatisticsQueryHandler> _logger;

    public GetMarketStatisticsQueryHandler(
        IStockQueryRepository stockRepository,
        IMarketTickQueryRepository tickRepository,
        ILogger<GetMarketStatisticsQueryHandler> logger)
    {
        _stockRepository = stockRepository;
        _tickRepository = tickRepository;
        _logger = logger;
    }

    public async Task<MarketStatisticsDto> Handle(GetMarketStatisticsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calculating market statistics");

        var allStocks = await _stockRepository.GetAllAsync(cancellationToken);
        var activeStocks = allStocks.Where(s => s.IsActive).ToList();
        var latestTicks = await _tickRepository.GetLatestTicksAsync(1000, cancellationToken);

        var statistics = new MarketStatisticsDto
        {
            TotalStocks = allStocks.Count(),
            ActiveStocks = activeStocks.Count,
            TotalMarketCap = activeStocks.Sum(s => s.MarketCap),
            AveragePrice = activeStocks.Any() ? activeStocks.Average(s => s.CurrentPrice.Value) : 0,
            TotalVolume = latestTicks.Sum(t => t.Volume.Value),
            AdvancingStocks = latestTicks.Count(t => t.Change > 0),
            DecliningStocks = latestTicks.Count(t => t.Change < 0),
            UnchangedStocks = latestTicks.Count(t => t.Change == 0),
            GeneratedAt = DateTime.UtcNow
        };

        _logger.LogInformation("Market statistics calculated successfully");

        return statistics;
    }
}
