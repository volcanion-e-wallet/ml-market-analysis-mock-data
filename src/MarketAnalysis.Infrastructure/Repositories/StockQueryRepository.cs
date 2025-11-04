namespace MarketAnalysis.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;

public class StockQueryRepository : IStockQueryRepository
{
    private readonly ReadDbContext _context;

    public StockQueryRepository(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<Stock?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Symbol.Value == symbol.ToUpperInvariant(), cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetByExchangeAsync(string exchange, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.AsNoTracking()
            .Where(s => s.Exchange == exchange)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetBySectorAsync(string sector, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.AsNoTracking()
            .Where(s => s.Sector == sector)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetActiveStocksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.AsNoTracking()
            .Where(s => s.IsActive)
            .ToListAsync(cancellationToken);
    }
}
