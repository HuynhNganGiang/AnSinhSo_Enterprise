using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho số điện thoại.
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    /// <summary>
    /// Giá trị số điện thoại.
    /// </summary>
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Khởi tạo PhoneNumber.
    /// </summary>
    public static Result<PhoneNumber> Create(string value)
    {
        value = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PhoneNumber>(Error.Validation("PhoneNumber.Empty", "Số điện thoại không được để trống."));
        }

        if (value.Length < 10 || value.Length > 15 || !value.All(char.IsDigit))
        {
            return Result.Failure<PhoneNumber>(Error.Validation("PhoneNumber.InvalidFormat", "Số điện thoại không hợp lệ."));
        }

        return Result.Success(new PhoneNumber(value));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
