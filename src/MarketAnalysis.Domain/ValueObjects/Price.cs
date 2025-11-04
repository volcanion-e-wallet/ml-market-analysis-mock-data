namespace MarketAnalysis.Domain.ValueObjects;

using MarketAnalysis.Domain.Common;

/// <summary>
/// Value object representing a stock price
/// </summary>
public class Price : ValueObject
{
    public decimal Value { get; private set; }
    public string Currency { get; private set; }

    private Price() { Currency = string.Empty; }

    private Price(decimal value, string currency)
    {
        if (value < 0)
            throw new ArgumentException("Price cannot be negative", nameof(value));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required", nameof(currency));

        Value = value;
        Currency = currency.ToUpperInvariant();
    }

    public static Price Create(decimal value, string currency = "USD")
    {
        return new Price(value, currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }

    public override string ToString()
    {
        return $"{Value:N2} {Currency}";
    }
}
