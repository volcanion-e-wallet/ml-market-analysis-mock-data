namespace MarketAnalysis.Domain.Repositories;

using MarketAnalysis.Domain.Entities;

/// <summary>
/// Repository interface for Stock queries (Read operations)
/// </summary>
public interface IStockQueryRepository
{
    Task<Stock?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetByExchangeAsync(string exchange, CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetBySectorAsync(string sector, CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetActiveStocksAsync(CancellationToken cancellationToken = default);
}
