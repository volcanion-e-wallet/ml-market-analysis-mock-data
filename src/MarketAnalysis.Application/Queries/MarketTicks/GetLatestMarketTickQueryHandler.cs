namespace MarketAnalysis.Application.Queries.MarketTicks;

using AutoMapper;
using MediatR;
using MarketAnalysis.Application.DTOs;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for GetLatestMarketTickQuery
/// </summary>
public class GetLatestMarketTickQueryHandler : IRequestHandler<GetLatestMarketTickQuery, MarketTickDto?>
{
    private readonly IMarketTickQueryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetLatestMarketTickQueryHandler> _logger;

    public GetLatestMarketTickQueryHandler(
        IMarketTickQueryRepository repository,
        IMapper mapper,
        ILogger<GetLatestMarketTickQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MarketTickDto?> Handle(GetLatestMarketTickQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting latest market tick for symbol: {Symbol}", request.Symbol);

        var tick = await _repository.GetLatestBySymbolAsync(request.Symbol, cancellationToken);
        
        if (tick == null)
        {
            _logger.LogWarning("No market tick found for symbol: {Symbol}", request.Symbol);
            return null;
        }

        return _mapper.Map<MarketTickDto>(tick);
    }
}
