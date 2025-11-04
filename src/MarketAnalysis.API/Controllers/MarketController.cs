namespace MarketAnalysis.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MarketAnalysis.Application.Queries.MarketTicks;
using MarketAnalysis.Application.Commands.Market;
using MarketAnalysis.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MarketController> _logger;

    public MarketController(IMediator mediator, ILogger<MarketController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get latest market tick for a specific symbol
    /// </summary>
    [HttpGet("realtime/{symbol}")]
    [ProducesResponseType(typeof(MarketTickDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MarketTickDto>> GetLatestTick(string symbol)
    {
        var query = new GetLatestMarketTickQuery(symbol);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound($"No market data found for symbol {symbol}");

        return Ok(result);
    }

    /// <summary>
    /// Get market statistics
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(MarketStatisticsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<MarketStatisticsDto>> GetStatistics()
    {
        var query = new GetMarketStatisticsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a single market tick
    /// </summary>
    [HttpPost("ticks")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateTick([FromBody] CreateMarketTickCommand command)
    {
        try
        {
            var tickId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetLatestTick), new { symbol = command.Symbol }, tickId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating market tick for {Symbol}", command.Symbol);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Create multiple market ticks in batch
    /// </summary>
    [HttpPost("ticks/batch")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateTicksBatch([FromBody] CreateMarketTicksBatchCommand command)
    {
        try
        {
            var count = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetStatistics), null, new { count, message = $"Created {count} market ticks" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating market ticks batch");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "Market Analysis API"
        });
    }
}
