namespace MarketAnalysis.SimulationJob.Services;

using MarketAnalysis.SimulationJob.Models;

public interface IMarketApiClient
{
    Task<List<StockDto>> GetAllStocksAsync(CancellationToken cancellationToken = default);
    Task<StockDto?> GetStockBySymbolAsync(string symbol, CancellationToken cancellationToken = default);
    Task<Guid> CreateStockAsync(CreateStockRequest request, CancellationToken cancellationToken = default);
    Task CreateMarketTickAsync(MarketTickRequest request, CancellationToken cancellationToken = default);
    Task CreateMarketTicksBatchAsync(BatchMarketTicksRequest request, CancellationToken cancellationToken = default);
    Task UpdateStockPriceAsync(string symbol, decimal newPrice, CancellationToken cancellationToken = default);
    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
}
