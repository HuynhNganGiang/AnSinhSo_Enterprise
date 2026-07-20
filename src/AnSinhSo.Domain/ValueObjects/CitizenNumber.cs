using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho số CCCD (Citizen Number).
/// </summary>
public sealed class CitizenNumber : ValueObject
{
    /// <summary>
    /// Giá trị CCCD.
    /// </summary>
    public string Value { get; }

    private CitizenNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Khởi tạo CitizenNumber.
    /// </summary>
    public static Result<CitizenNumber> Create(string value)
    {
        value = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<CitizenNumber>(Error.Validation("CitizenNumber.Empty", "Số CCCD không được để trống."));
        }

        if (value.Length != 12 || !value.All(char.IsDigit))
        {
            return Result.Failure<CitizenNumber>(Error.Validation("CitizenNumber.InvalidFormat", "Số CCCD phải bao gồm đúng 12 chữ số."));
        }

        return Result.Success(new CitizenNumber(value));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
