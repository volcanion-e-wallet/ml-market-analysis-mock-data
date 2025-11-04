namespace MarketAnalysis.Application.Commands.Stocks;

using MediatR;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for CreateStockCommand
/// </summary>
public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, Guid>
{
    private readonly IStockCommandRepository _repository;
    private readonly ILogger<CreateStockCommandHandler> _logger;

    public CreateStockCommandHandler(
        IStockCommandRepository repository,
        ILogger<CreateStockCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating stock with symbol: {Symbol}", request.Symbol);

        // Check if stock already exists
        var existingStock = await _repository.GetBySymbolAsync(request.Symbol, cancellationToken);
        if (existingStock != null)
        {
            throw new InvalidOperationException($"Stock with symbol {request.Symbol} already exists");
        }

        // Create new stock
        var stock = Stock.Create(
            request.Symbol,
            request.Name,
            request.Exchange,
            request.Sector,
            request.Industry,
            request.InitialPrice);

        await _repository.AddAsync(stock, cancellationToken);

        _logger.LogInformation("Stock created successfully with ID: {StockId}", stock.Id);

        return stock.Id;
    }
}
