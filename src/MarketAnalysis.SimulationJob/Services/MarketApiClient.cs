namespace MarketAnalysis.SimulationJob.Services;

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarketAnalysis.SimulationJob.Configuration;
using MarketAnalysis.SimulationJob.Models;

public class MarketApiClient : IMarketApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MarketApiClient> _logger;
    private readonly ApiSettings _settings;
    private readonly JsonSerializerOptions _jsonOptions;

    public MarketApiClient(
        HttpClient httpClient,
        ILogger<MarketApiClient> logger,
        IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = settings.Value;
        
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<StockDto>> GetAllStocksAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/stocks", cancellationToken);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<List<StockDto>>(_jsonOptions, cancellationToken) 
                   ?? new List<StockDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all stocks");
            throw;
        }
    }

    public async Task<StockDto?> GetStockBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/stocks/{symbol}", cancellationToken);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
                
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StockDto>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stock {Symbol}", symbol);
            throw;
        }
    }

    public async Task<Guid> CreateStockAsync(CreateStockRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/stocks", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<Guid>(_jsonOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create stock {Symbol}", request.Symbol);
            throw;
        }
    }

    public async Task CreateMarketTickAsync(MarketTickRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/market/ticks", request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create market tick for {Symbol}", request.Symbol);
            throw;
        }
    }

    public async Task CreateMarketTicksBatchAsync(BatchMarketTicksRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/market/ticks/batch", request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create batch market ticks");
            throw;
        }
    }

    public async Task UpdateStockPriceAsync(string symbol, decimal newPrice, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/stocks/{symbol}/price", newPrice, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update stock price for {Symbol}", symbol);
            throw;
        }
    }

    public async Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("/health", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
