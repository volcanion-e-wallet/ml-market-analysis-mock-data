namespace MarketAnalysis.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;

public class TradingHistoryQueryRepository : ITradingHistoryQueryRepository
{
    private readonly ReadDbContext _context;

    public TradingHistoryQueryRepository(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TradingHistory>> GetBySymbolAsync(string symbol, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.TradingHistories.AsNoTracking()
            .Where(h => h.Symbol.Value == symbol.ToUpperInvariant() && h.Date >= from && h.Date <= to)
            .OrderBy(h => h.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TradingHistory>> GetLatestBySymbolAsync(string symbol, int days, CancellationToken cancellationToken = default)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);
        return await _context.TradingHistories.AsNoTracking()
            .Where(h => h.Symbol.Value == symbol.ToUpperInvariant() && h.Date >= fromDate)
            .OrderByDescending(h => h.Date)
            .Take(days)
            .ToListAsync(cancellationToken);
    }
}
