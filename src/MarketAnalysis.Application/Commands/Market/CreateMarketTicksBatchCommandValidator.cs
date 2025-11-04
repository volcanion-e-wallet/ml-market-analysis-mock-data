using FluentValidation;

namespace MarketAnalysis.Application.Commands.Market;

public class CreateMarketTicksBatchCommandValidator : AbstractValidator<CreateMarketTicksBatchCommand>
{
    public CreateMarketTicksBatchCommandValidator()
    {
        RuleFor(x => x.Ticks)
            .NotEmpty().WithMessage("Ticks list cannot be empty")
            .Must(ticks => ticks.Count <= 1000).WithMessage("Cannot process more than 1000 ticks at once");

        RuleForEach(x => x.Ticks).ChildRules(tick =>
        {
            tick.RuleFor(t => t.Symbol)
                .NotEmpty().WithMessage("Symbol is required")
                .MaximumLength(10).WithMessage("Symbol cannot exceed 10 characters");

            tick.RuleFor(t => t.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            tick.RuleFor(t => t.Volume)
                .GreaterThan(0).WithMessage("Volume must be greater than 0");

            tick.RuleFor(t => t.High)
                .GreaterThanOrEqualTo(t => t.Low).WithMessage("High must be greater than or equal to Low");
        });
    }
}
