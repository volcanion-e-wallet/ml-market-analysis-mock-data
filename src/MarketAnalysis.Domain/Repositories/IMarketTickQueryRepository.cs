namespace MarketAnalysis.Domain.Repositories;

using MarketAnalysis.Domain.Entities;

/// <summary>
/// Repository interface for MarketTick queries (Read operations)
/// </summary>
public interface IMarketTickQueryRepository
{
    Task<MarketTick?> GetLatestBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
    Task<IEnumerable<MarketTick>> GetBySymbolAsync(string symbol, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IEnumerable<MarketTick>> GetLatestTicksAsync(int count = 100, CancellationToken cancellationToken = default);
}
