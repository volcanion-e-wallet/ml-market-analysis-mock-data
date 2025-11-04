using FluentValidation;

namespace MarketAnalysis.Application.Commands.Market;

public class CreateMarketTickCommandValidator : AbstractValidator<CreateMarketTickCommand>
{
    public CreateMarketTickCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Symbol is required")
            .MaximumLength(10).WithMessage("Symbol cannot exceed 10 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Volume)
            .GreaterThan(0).WithMessage("Volume must be greater than 0");

        RuleFor(x => x.High)
            .GreaterThanOrEqualTo(x => x.Low).WithMessage("High must be greater than or equal to Low");

        RuleFor(x => x.Low)
            .LessThanOrEqualTo(x => x.High).WithMessage("Low must be less than or equal to High");

        RuleFor(x => x.Open)
            .GreaterThan(0).WithMessage("Open price must be greater than 0");

        RuleFor(x => x.PreviousClose)
            .GreaterThan(0).WithMessage("Previous close must be greater than 0");
    }
}
