namespace MarketAnalysis.Domain.ValueObjects;

using MarketAnalysis.Domain.Common;

/// <summary>
/// Value object representing trading volume
/// </summary>
public class Volume : ValueObject
{
    public long Value { get; private set; }

    private Volume() { }

    private Volume(long value)
    {
        if (value < 0)
            throw new ArgumentException("Volume cannot be negative", nameof(value));

        Value = value;
    }

    public static Volume Create(long value)
    {
        return new Volume(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString("N0");
    }

    public static implicit operator long(Volume volume) => volume.Value;
}
