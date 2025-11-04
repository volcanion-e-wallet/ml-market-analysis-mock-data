namespace MarketAnalysis.Domain.Repositories;

using MarketAnalysis.Domain.Entities;

/// <summary>
/// Repository interface for TradingHistory (Write operations)
/// </summary>
public interface ITradingHistoryCommandRepository
{
    Task AddAsync(TradingHistory history, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TradingHistory> histories, CancellationToken cancellationToken = default);
}
