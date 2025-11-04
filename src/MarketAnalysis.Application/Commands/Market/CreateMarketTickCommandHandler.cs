using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MarketAnalysis.Application.Commands.Market;

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
        try
        {
            var marketTick = MarketTick.Create(
                request.Symbol,
                request.Price,
                request.Volume,
                request.High,
                request.Low,
                request.Open,
                request.PreviousClose,
                request.Timestamp
            );

            await _repository.AddAsync(marketTick);
            
            _logger.LogInformation("Created market tick for {Symbol} at {Price}", request.Symbol, request.Price);
            
            return marketTick.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create market tick for {Symbol}", request.Symbol);
            throw;
        }
    }
}
