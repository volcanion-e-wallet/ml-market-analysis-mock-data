namespace MarketAnalysis.Application.Commands.MarketTicks;

using MediatR;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for CreateMarketTickCommand
/// </summary>
public class CreateMarketTickCommandHandler : IRequestHandler<CreateMarketTickCommand, Guid>
{
    private readonly IMarketTickCommandRepository _repository;
    private readonly ILogger<CreateMarketTickCommandHandler> _logger;

    public CreateMarketTickCommandHandler(
        IMarketTickCommandRepository repository,
        ILogger<CreateMarketTickCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateMarketTickCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating market tick for symbol: {Symbol}", request.Symbol);

        var tick = MarketTick.Create(
            request.Symbol,
            request.Price,
            request.Volume,
            request.High,
            request.Low,
            request.Open,
            request.PreviousClose,
            request.Timestamp);

        await _repository.AddAsync(tick, cancellationToken);

        _logger.LogInformation("Market tick created successfully with ID: {TickId}", tick.Id);

        return tick.Id;
    }
}
