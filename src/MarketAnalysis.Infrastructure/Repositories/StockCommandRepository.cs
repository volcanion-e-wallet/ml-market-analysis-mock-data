namespace MarketAnalysis.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;

public class StockCommandRepository : IStockCommandRepository
{
    private readonly WriteDbContext _context;

    public StockCommandRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Stock?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(s => s.Symbol.Value == symbol.ToUpperInvariant(), cancellationToken);
    }

    public async Task AddAsync(Stock stock, CancellationToken cancellationToken = default)
    {
        await _context.Stocks.AddAsync(stock, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Stock stock, CancellationToken cancellationToken = default)
    {
        _context.Stocks.Update(stock);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var stock = await GetByIdAsync(id, cancellationToken);
        if (stock != null)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return await _context.Stocks
            .AnyAsync(s => s.Symbol.Value == symbol.ToUpperInvariant(), cancellationToken);
    }
}
