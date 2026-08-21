using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object d?i di?n cho ph?n tram.
/// </summary>
public sealed class Percentage : ValueObject
{
    /// <summary>
    /// Giá tr? ph?n tram (t? 0 d?n 100).
    /// </summary>
    public decimal Value { get; }

    private Percentage(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// Kh?i t?o Percentage.
    /// </summary>
    public static Result<Percentage> Create(decimal value)
    {
        if (value < 0 || value > 100)
        {
            return Result.Failure<Percentage>(Error.Validation("Percentage.OutOfRange", "Ph?n tram ph?i n?m trong kho?ng t? 0 d?n 100."));
        }

        return Result.Success(new Percentage(value));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
