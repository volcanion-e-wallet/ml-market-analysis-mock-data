namespace MarketAnalysis.Domain.Repositories;

using MarketAnalysis.Domain.Entities;

/// <summary>
/// Repository interface for TradingHistory queries (Read operations)
/// </summary>
public interface ITradingHistoryQueryRepository
{
    Task<IEnumerable<TradingHistory>> GetBySymbolAsync(string symbol, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradingHistory>> GetLatestBySymbolAsync(string symbol, int days, CancellationToken cancellationToken = default);
}
