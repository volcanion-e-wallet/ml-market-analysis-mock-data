using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using MarketAnalysis.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MarketAnalysis.Application.Commands.Market;

public class CreateMarketTicksBatchCommandHandler : IRequestHandler<CreateMarketTicksBatchCommand, int>
{
    private readonly IMarketTickCommandRepository _repository;
    private readonly ILogger<CreateMarketTicksBatchCommandHandler> _logger;

    public CreateMarketTicksBatchCommandHandler(
        IMarketTickCommandRepository repository,
        ILogger<CreateMarketTicksBatchCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> Handle(CreateMarketTicksBatchCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var marketTicks = new List<MarketTick>();

            foreach (var tickDto in request.Ticks)
            {
                var marketTick = MarketTick.Create(
                    tickDto.Symbol,
                    tickDto.Price,
                    tickDto.Volume,
                    tickDto.High,
                    tickDto.Low,
                    tickDto.Open,
                    tickDto.PreviousClose,
                    tickDto.Timestamp
                );

                marketTicks.Add(marketTick);
            }

            await _repository.AddRangeAsync(marketTicks);
            
            _logger.LogInformation("Created {Count} market ticks in batch", marketTicks.Count);
            
            return marketTicks.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create market ticks batch");
            throw;
        }
    }
}
