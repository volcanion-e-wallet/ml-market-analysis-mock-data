namespace MarketAnalysis.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Infrastructure.Persistence;
using MarketAnalysis.Infrastructure.Repositories;
using MarketAnalysis.Infrastructure.Services;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Database Contexts
        services.AddDbContext<WriteDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WriteDatabase")));

        services.AddDbContext<ReadDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ReadDatabase"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        // Register Repositories
        services.AddScoped<IStockCommandRepository, StockCommandRepository>();
        services.AddScoped<IStockQueryRepository, StockQueryRepository>();
        services.AddScoped<IMarketTickCommandRepository, MarketTickCommandRepository>();
        services.AddScoped<IMarketTickQueryRepository, MarketTickQueryRepository>();
        services.AddScoped<ITradingHistoryCommandRepository, TradingHistoryCommandRepository>();
        services.AddScoped<ITradingHistoryQueryRepository, TradingHistoryQueryRepository>();

        // Add Redis
        var redisConnectionString = configuration["Redis:ConnectionString"];
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddScoped<ICacheService, RedisCacheService>();
        }

        // Add Background Services (Disabled - Using SimulationJob instead)
        // services.AddHostedService<MarketDataGeneratorService>();

        return services;
    }
}
