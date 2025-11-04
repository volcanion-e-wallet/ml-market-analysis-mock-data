namespace MarketAnalysis.Infrastructure.Repositories;

using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;

public class MarketTickCommandRepository : IMarketTickCommandRepository
{
    private readonly WriteDbContext _context;

    public MarketTickCommandRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MarketTick tick, CancellationToken cancellationToken = default)
    {
        await _context.MarketTicks.AddAsync(tick, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<MarketTick> ticks, CancellationToken cancellationToken = default)
    {
        await _context.MarketTicks.AddRangeAsync(ticks, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
