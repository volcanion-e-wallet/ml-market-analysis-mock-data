using MediatR;

namespace MarketAnalysis.Application.Commands.Market;

public record CreateMarketTicksBatchCommand(
    List<CreateMarketTickRequest> Ticks
) : IRequest<int>;

public record CreateMarketTickRequest(
    string Symbol,
    decimal Price,
    long Volume,
    decimal High,
    decimal Low,
    decimal Open,
    decimal PreviousClose,
    DateTime? Timestamp = null
);
