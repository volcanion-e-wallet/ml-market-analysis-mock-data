namespace MarketAnalysis.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MarketAnalysis.Application.Commands.Stocks;
using MarketAnalysis.Application.Queries.Stocks;
using MarketAnalysis.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StocksController> _logger;

    public StocksController(IMediator mediator, ILogger<StocksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all stocks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StockDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetAll()
    {
        var query = new GetAllStocksQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get stock by symbol
    /// </summary>
    [HttpGet("{symbol}")]
    [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StockDto>> GetBySymbol(string symbol)
    {
        var query = new GetStockBySymbolQuery(symbol);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound($"Stock with symbol {symbol} not found");

        return Ok(result);
    }

    /// <summary>
    /// Create a new stock
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateStockCommand command)
    {
        try
        {
            var stockId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBySymbol), new { symbol = command.Symbol }, stockId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update stock price
    /// </summary>
    [HttpPut("{symbol}/price")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrice(string symbol, [FromBody] decimal newPrice)
    {
        try
        {
            var command = new UpdateStockPriceCommand { Symbol = symbol, NewPrice = newPrice };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
