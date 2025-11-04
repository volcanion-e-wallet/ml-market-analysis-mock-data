namespace MarketAnalysis.Domain.Repositories;

using MarketAnalysis.Domain.Entities;

/// <summary>
/// Repository interface for Stock aggregate (Write operations)
/// </summary>
public interface IStockCommandRepository
{
    Task<Stock?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
    Task AddAsync(Stock stock, CancellationToken cancellationToken = default);
    Task UpdateAsync(Stock stock, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string symbol, CancellationToken cancellationToken = default);
}
