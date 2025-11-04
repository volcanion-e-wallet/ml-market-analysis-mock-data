namespace MarketAnalysis.Infrastructure.Repositories;

using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;

public class TradingHistoryCommandRepository : ITradingHistoryCommandRepository
{
    private readonly WriteDbContext _context;

    public TradingHistoryCommandRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TradingHistory history, CancellationToken cancellationToken = default)
    {
        await _context.TradingHistories.AddAsync(history, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TradingHistory> histories, CancellationToken cancellationToken = default)
    {
        await _context.TradingHistories.AddRangeAsync(histories, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
