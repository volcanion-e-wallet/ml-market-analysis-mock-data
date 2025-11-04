namespace MarketAnalysis.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using MarketAnalysis.Domain.Entities;

/// <summary>
/// Read Database Context for Query operations
/// </summary>
public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<MarketTick> MarketTicks => Set<MarketTick>();
    public DbSet<TradingHistory> TradingHistories => Set<TradingHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Use same configuration as WriteDbContext
        // In a real scenario, you might have different optimizations for read operations
        
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("stocks");
            entity.HasKey(e => e.Id);
            
            entity.OwnsOne(e => e.Symbol, symbol =>
            {
                symbol.Property(s => s.Value).HasColumnName("symbol").HasMaxLength(10).IsRequired();
            });

            entity.OwnsOne(e => e.CurrentPrice, price =>
            {
                price.Property(p => p.Value).HasColumnName("current_price").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("currency").HasMaxLength(3).IsRequired();
            });
        });

        modelBuilder.Entity<MarketTick>(entity =>
        {
            entity.ToTable("market_ticks");
            entity.HasKey(e => e.Id);

            entity.OwnsOne(e => e.Symbol, symbol =>
            {
                symbol.Property(s => s.Value).HasColumnName("symbol").HasMaxLength(10).IsRequired();
            });

            entity.OwnsOne(e => e.Price, price =>
            {
                price.Property(p => p.Value).HasColumnName("price").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.Volume, volume =>
            {
                volume.Property(v => v.Value).HasColumnName("volume").IsRequired();
            });

            entity.OwnsOne(e => e.High, price =>
            {
                price.Property(p => p.Value).HasColumnName("high").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("high_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.Low, price =>
            {
                price.Property(p => p.Value).HasColumnName("low").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("low_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.Open, price =>
            {
                price.Property(p => p.Value).HasColumnName("open").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("open_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.PreviousClose, price =>
            {
                price.Property(p => p.Value).HasColumnName("previous_close").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("previous_close_currency").HasMaxLength(3).IsRequired();
            });
        });

        modelBuilder.Entity<TradingHistory>(entity =>
        {
            entity.ToTable("trading_histories");
            entity.HasKey(e => e.Id);

            entity.OwnsOne(e => e.Symbol, symbol =>
            {
                symbol.Property(s => s.Value).HasColumnName("symbol").HasMaxLength(10).IsRequired();
            });

            entity.OwnsOne(e => e.Open, price =>
            {
                price.Property(p => p.Value).HasColumnName("open").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("open_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.High, price =>
            {
                price.Property(p => p.Value).HasColumnName("high").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("high_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.Low, price =>
            {
                price.Property(p => p.Value).HasColumnName("low").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("low_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.Close, price =>
            {
                price.Property(p => p.Value).HasColumnName("close").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("close_currency").HasMaxLength(3).IsRequired();
            });

            entity.OwnsOne(e => e.Volume, volume =>
            {
                volume.Property(v => v.Value).HasColumnName("volume").IsRequired();
            });

            entity.OwnsOne(e => e.AdjustedClose, price =>
            {
                price.Property(p => p.Value).HasColumnName("adjusted_close").HasColumnType("decimal(18,2)").IsRequired();
                price.Property(p => p.Currency).HasColumnName("adjusted_close_currency").HasMaxLength(3).IsRequired();
            });
        });
    }
}
