namespace MarketAnalysis.Application.Queries.Stocks;

using AutoMapper;
using MediatR;
using MarketAnalysis.Application.DTOs;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for GetAllStocksQuery
/// </summary>
public class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, IEnumerable<StockDto>>
{
    private readonly IStockQueryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllStocksQueryHandler> _logger;

    public GetAllStocksQueryHandler(
        IStockQueryRepository repository,
        IMapper mapper,
        ILogger<GetAllStocksQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<StockDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all stocks");

        var stocks = await _repository.GetAllAsync(cancellationToken);
        var stockDtos = _mapper.Map<IEnumerable<StockDto>>(stocks);

        _logger.LogInformation("Retrieved {Count} stocks", stockDtos.Count());

        return stockDtos;
    }
}
