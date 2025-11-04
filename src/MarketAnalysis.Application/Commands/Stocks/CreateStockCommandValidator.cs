namespace MarketAnalysis.Application.Commands.Stocks;

using FluentValidation;

/// <summary>
/// Validator for CreateStockCommand
/// </summary>
public class CreateStockCommandValidator : AbstractValidator<CreateStockCommand>
{
    public CreateStockCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Symbol is required")
            .MaximumLength(10).WithMessage("Symbol cannot exceed 10 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Exchange)
            .NotEmpty().WithMessage("Exchange is required")
            .MaximumLength(50).WithMessage("Exchange cannot exceed 50 characters");

        RuleFor(x => x.Sector)
            .NotEmpty().WithMessage("Sector is required")
            .MaximumLength(100).WithMessage("Sector cannot exceed 100 characters");

        RuleFor(x => x.Industry)
            .NotEmpty().WithMessage("Industry is required")
            .MaximumLength(100).WithMessage("Industry cannot exceed 100 characters");

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0).WithMessage("Initial price must be greater than 0");
    }
}
