using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho phần trăm.
/// </summary>
public sealed class Percentage : ValueObject
{
    /// <summary>
    /// Giá trị phần trăm (từ 0 đến 100).
    /// </summary>
    public decimal Value { get; }

    private Percentage(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// Khởi tạo Percentage.
    /// </summary>
    public static Result<Percentage> Create(decimal value)
    {
        if (value < 0 || value > 100)
        {
            return Result.Failure<Percentage>(Error.Validation("Percentage.OutOfRange", "Phần trăm phải nằm trong khoảng từ 0 đến 100."));
        }

        return Result.Success(new Percentage(value));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
