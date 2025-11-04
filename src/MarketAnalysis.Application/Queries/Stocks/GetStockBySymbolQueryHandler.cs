namespace MarketAnalysis.Application.Queries.Stocks;

using AutoMapper;
using MediatR;
using MarketAnalysis.Application.DTOs;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for GetStockBySymbolQuery
/// </summary>
public class GetStockBySymbolQueryHandler : IRequestHandler<GetStockBySymbolQuery, StockDto?>
{
    private readonly IStockQueryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStockBySymbolQueryHandler> _logger;

    public GetStockBySymbolQueryHandler(
        IStockQueryRepository repository,
        IMapper mapper,
        ILogger<GetStockBySymbolQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StockDto?> Handle(GetStockBySymbolQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting stock by symbol: {Symbol}", request.Symbol);

        var stock = await _repository.GetBySymbolAsync(request.Symbol, cancellationToken);
        
        if (stock == null)
        {
            _logger.LogWarning("Stock not found: {Symbol}", request.Symbol);
            return null;
        }

        return _mapper.Map<StockDto>(stock);
    }
}
