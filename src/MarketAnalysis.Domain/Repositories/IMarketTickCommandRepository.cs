namespace MarketAnalysis.Domain.Repositories;

using MarketAnalysis.Domain.Entities;

/// <summary>
/// Repository interface for MarketTick (Write operations)
/// </summary>
public interface IMarketTickCommandRepository
{
    Task AddAsync(MarketTick tick, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<MarketTick> ticks, CancellationToken cancellationToken = default);
}
