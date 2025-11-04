using MarketAnalysis.Domain.ValueObjects;
using MediatR;

namespace MarketAnalysis.Application.Commands.Market;

public record CreateMarketTickCommand(
    string Symbol,
    decimal Price,
    long Volume,
    decimal High,
    decimal Low,
    decimal Open,
    decimal PreviousClose,
    DateTime? Timestamp = null
) : IRequest<Guid>;
