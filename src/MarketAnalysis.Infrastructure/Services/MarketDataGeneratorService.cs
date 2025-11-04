namespace MarketAnalysis.Infrastructure.Services;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;

/// <summary>
/// Background service to generate mock real-time market data
/// </summary>
public class MarketDataGeneratorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MarketDataGeneratorService> _logger;
    private readonly Random _random = new();
    private readonly Dictionary<string, decimal> _stockPrices = new();

    public MarketDataGeneratorService(
        IServiceProvider serviceProvider,
        ILogger<MarketDataGeneratorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Market Data Generator Service starting...");

        // Wait for 30 seconds to allow database initialization
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        await InitializeSampleStocksAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateMarketTicksAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating market data");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        _logger.LogInformation("Market Data Generator Service stopping...");
    }

    private async Task InitializeSampleStocksAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IStockCommandRepository>();

        var sampleStocks = new[]
        {
            ("AAPL", "Apple Inc.", "NASDAQ", "Technology", "Consumer Electronics", 175.50m),
            ("GOOGL", "Alphabet Inc.", "NASDAQ", "Technology", "Internet Services", 140.25m),
            ("MSFT", "Microsoft Corp.", "NASDAQ", "Technology", "Software", 380.75m),
            ("AMZN", "Amazon.com Inc.", "NASDAQ", "Consumer Cyclical", "Internet Retail", 145.80m),
            ("TSLA", "Tesla Inc.", "NASDAQ", "Consumer Cyclical", "Auto Manufacturers", 245.30m),
            ("META", "Meta Platforms Inc.", "NASDAQ", "Technology", "Social Media", 485.60m),
            ("NVDA", "NVIDIA Corp.", "NASDAQ", "Technology", "Semiconductors", 495.25m),
            ("JPM", "JPMorgan Chase & Co.", "NYSE", "Financial", "Banks", 185.40m),
            ("V", "Visa Inc.", "NYSE", "Financial", "Credit Services", 265.90m),
            ("WMT", "Walmart Inc.", "NYSE", "Consumer Defensive", "Retail", 165.75m)
        };

        foreach (var (symbol, name, exchange, sector, industry, price) in sampleStocks)
        {
            try
            {
                if (!await repository.ExistsAsync(symbol, cancellationToken))
                {
                    var stock = Stock.Create(symbol, name, exchange, sector, industry, price);
                    await repository.AddAsync(stock, cancellationToken);
                    _stockPrices[symbol] = price;
                    _logger.LogInformation("Created stock: {Symbol}", symbol);
                }
                else
                {
                    var stock = await repository.GetBySymbolAsync(symbol, cancellationToken);
                    if (stock != null)
                    {
                        _stockPrices[symbol] = stock.CurrentPrice.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing stock {Symbol}", symbol);
            }
        }
    }

    private async Task GenerateMarketTicksAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var tickRepository = scope.ServiceProvider.GetRequiredService<IMarketTickCommandRepository>();
        var stockRepository = scope.ServiceProvider.GetRequiredService<IStockCommandRepository>();

        var ticks = new List<MarketTick>();

        foreach (var (symbol, currentPrice) in _stockPrices.ToList())
        {
            try
            {
                // Simulate price movement (-2% to +2%)
                var priceChange = (decimal)(_random.NextDouble() * 0.04 - 0.02);
                var newPrice = currentPrice * (1 + priceChange);
                newPrice = Math.Round(newPrice, 2);

                // Simulate daily range
                var high = newPrice * (1 + (decimal)(_random.NextDouble() * 0.01));
                var low = newPrice * (1 - (decimal)(_random.NextDouble() * 0.01));
                var open = currentPrice;
                var previousClose = currentPrice;

                // Generate random volume
                var volume = _random.Next(100000, 10000000);

                var tick = MarketTick.Create(
                    symbol,
                    newPrice,
                    volume,
                    Math.Round(high, 2),
                    Math.Round(low, 2),
                    Math.Round(open, 2),
                    Math.Round(previousClose, 2),
                    DateTime.UtcNow
                );

                ticks.Add(tick);
                _stockPrices[symbol] = newPrice;

                // Update stock price
                var stock = await stockRepository.GetBySymbolAsync(symbol, cancellationToken);
                if (stock != null)
                {
                    stock.UpdatePrice(newPrice);
                    await stockRepository.UpdateAsync(stock, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating tick for {Symbol}", symbol);
            }
        }

        if (ticks.Any())
        {
            await tickRepository.AddRangeAsync(ticks, cancellationToken);
            _logger.LogInformation("Generated {Count} market ticks", ticks.Count);
        }
    }
}
