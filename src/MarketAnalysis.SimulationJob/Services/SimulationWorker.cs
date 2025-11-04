namespace MarketAnalysis.SimulationJob.Services;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarketAnalysis.SimulationJob.Configuration;
using MarketAnalysis.SimulationJob.Models;

public class SimulationWorker : BackgroundService
{
    private readonly ILogger<SimulationWorker> _logger;
    private readonly IMarketApiClient _apiClient;
    private readonly IStockDataProvider _stockDataProvider;
    private readonly IMarketSimulator _marketSimulator;
    private readonly SimulationSettings _settings;
    private readonly Dictionary<string, decimal> _stockPrices = new();

    public SimulationWorker(
        ILogger<SimulationWorker> logger,
        IMarketApiClient apiClient,
        IStockDataProvider stockDataProvider,
        IMarketSimulator marketSimulator,
        IOptions<SimulationSettings> settings)
    {
        _logger = logger;
        _apiClient = apiClient;
        _stockDataProvider = stockDataProvider;
        _marketSimulator = marketSimulator;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Market Simulation Worker starting...");
        _logger.LogInformation("Settings: Interval={Interval}s, BatchMode={BatchMode}", 
            _settings.IntervalSeconds, _settings.EnableBatchMode);

        // Wait for API to be ready
        _logger.LogInformation("Waiting {Seconds}s for API to be ready...", _settings.InitialDelaySeconds);
        await Task.Delay(TimeSpan.FromSeconds(_settings.InitialDelaySeconds), stoppingToken);

        // Check API health
        if (!await WaitForApiHealthAsync(stoppingToken))
        {
            _logger.LogError("API is not healthy. Stopping simulation.");
            return;
        }

        // Initialize stocks
        await InitializeStocksAsync(stoppingToken);

        // Main simulation loop
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SimulateMarketTicksAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(_settings.IntervalSeconds), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in simulation loop");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        _logger.LogInformation("Market Simulation Worker stopping...");
    }

    private async Task<bool> WaitForApiHealthAsync(CancellationToken cancellationToken)
    {
        var maxRetries = 10;
        var retryCount = 0;

        while (retryCount < maxRetries && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (await _apiClient.HealthCheckAsync(cancellationToken))
                {
                    _logger.LogInformation("API is healthy and ready");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Health check failed, retrying... ({Count}/{Max})", retryCount + 1, maxRetries);
            }

            retryCount++;
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }

        return false;
    }

    private async Task InitializeStocksAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing sample stocks...");

        var sampleStocks = _stockDataProvider.GetSampleStocks();

        foreach (var stockRequest in sampleStocks)
        {
            try
            {
                var existingStock = await _apiClient.GetStockBySymbolAsync(stockRequest.Symbol, cancellationToken);

                if (existingStock == null)
                {
                    await _apiClient.CreateStockAsync(stockRequest, cancellationToken);
                    _stockPrices[stockRequest.Symbol] = stockRequest.InitialPrice;
                    _logger.LogInformation("Created stock: {Symbol} at ${Price}", stockRequest.Symbol, stockRequest.InitialPrice);
                }
                else
                {
                    _stockPrices[existingStock.Symbol] = existingStock.CurrentPrice;
                    _logger.LogInformation("Stock already exists: {Symbol} at ${Price}", existingStock.Symbol, existingStock.CurrentPrice);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize stock {Symbol}", stockRequest.Symbol);
            }
        }

        _logger.LogInformation("Stock initialization completed. Tracking {Count} stocks", _stockPrices.Count);
    }

    private async Task SimulateMarketTicksAsync(CancellationToken cancellationToken)
    {
        if (_stockPrices.Count == 0)
        {
            _logger.LogWarning("No stocks to simulate");
            return;
        }

        var stocks = await _apiClient.GetAllStocksAsync(cancellationToken);
        var ticks = new List<MarketTickRequest>();

        foreach (var stock in stocks.Where(s => s.IsActive))
        {
            try
            {
                if (!_stockPrices.ContainsKey(stock.Symbol))
                {
                    _stockPrices[stock.Symbol] = stock.CurrentPrice;
                }

                var currentPrice = _stockPrices[stock.Symbol];
                var tick = _marketSimulator.GenerateTick(stock, currentPrice);
                
                ticks.Add(tick);
                _stockPrices[stock.Symbol] = tick.Price;

                // Update stock price via API
                await _apiClient.UpdateStockPriceAsync(stock.Symbol, tick.Price, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate tick for {Symbol}", stock.Symbol);
            }
        }

        if (ticks.Any())
        {
            if (_settings.EnableBatchMode)
            {
                // Send all ticks in one batch
                await _apiClient.CreateMarketTicksBatchAsync(new BatchMarketTicksRequest { Ticks = ticks }, cancellationToken);
                _logger.LogInformation("Generated and sent {Count} market ticks in batch mode", ticks.Count);
            }
            else
            {
                // Send ticks individually
                foreach (var tick in ticks)
                {
                    await _apiClient.CreateMarketTickAsync(tick, cancellationToken);
                }
                _logger.LogInformation("Generated and sent {Count} market ticks individually", ticks.Count);
            }
        }
    }
}
