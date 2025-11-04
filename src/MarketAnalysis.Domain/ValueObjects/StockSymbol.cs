namespace MarketAnalysis.Domain.ValueObjects;

using MarketAnalysis.Domain.Common;

/// <summary>
/// Value object representing a stock symbol
/// </summary>
public class StockSymbol : ValueObject
{
    public string Value { get; private set; }

    private StockSymbol() { Value = string.Empty; }

    private StockSymbol(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Stock symbol cannot be empty", nameof(value));

        if (value.Length > 10)
            throw new ArgumentException("Stock symbol cannot exceed 10 characters", nameof(value));

        Value = value.ToUpperInvariant();
    }

    public static StockSymbol Create(string value)
    {
        return new StockSymbol(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(StockSymbol symbol) => symbol.Value;
}
