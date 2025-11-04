namespace MarketAnalysis.Application.Commands.Stocks;

using MediatR;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for UpdateStockPriceCommand
/// </summary>
public class UpdateStockPriceCommandHandler : IRequestHandler<UpdateStockPriceCommand, Unit>
{
    private readonly IStockCommandRepository _repository;
    private readonly ILogger<UpdateStockPriceCommandHandler> _logger;

    public UpdateStockPriceCommandHandler(
        IStockCommandRepository repository,
        ILogger<UpdateStockPriceCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateStockPriceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating price for stock: {Symbol}", request.Symbol);

        var stock = await _repository.GetBySymbolAsync(request.Symbol, cancellationToken);
        if (stock == null)
        {
            throw new InvalidOperationException($"Stock with symbol {request.Symbol} not found");
        }

        stock.UpdatePrice(request.NewPrice);
        await _repository.UpdateAsync(stock, cancellationToken);

        _logger.LogInformation("Stock price updated successfully for {Symbol}", request.Symbol);

        return Unit.Value;
    }
}
