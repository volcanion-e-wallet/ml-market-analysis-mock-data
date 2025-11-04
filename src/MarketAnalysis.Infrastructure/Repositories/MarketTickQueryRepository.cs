namespace MarketAnalysis.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;

public class MarketTickQueryRepository : IMarketTickQueryRepository
{
    private readonly ReadDbContext _context;

    public MarketTickQueryRepository(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<MarketTick?> GetLatestBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await _context.MarketTicks.AsNoTracking()
            .Where(t => t.Symbol.Value == symbol.ToUpperInvariant())
            .OrderByDescending(t => t.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<MarketTick>> GetBySymbolAsync(string symbol, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.MarketTicks.AsNoTracking()
            .Where(t => t.Symbol.Value == symbol.ToUpperInvariant() && t.Timestamp >= from && t.Timestamp <= to)
            .OrderBy(t => t.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MarketTick>> GetLatestTicksAsync(int count = 100, CancellationToken cancellationToken = default)
    {
        return await _context.MarketTicks.AsNoTracking()
            .OrderByDescending(t => t.Timestamp)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
