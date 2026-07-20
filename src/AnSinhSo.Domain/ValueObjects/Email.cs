using System.Collections.Generic;
using System.Text.RegularExpressions;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho địa chỉ Email.
/// </summary>
public sealed class Email : ValueObject
{
    /// <summary>
    /// Giá trị Email.
    /// </summary>
    public string Value { get; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Khởi tạo Email.
    /// </summary>
    public static Result<Email> Create(string value)
    {
        value = value?.Trim().ToLowerInvariant() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email>(Error.Validation("Email.Empty", "Email không được để trống."));
        }

        if (!EmailRegex.IsMatch(value))
        {
            return Result.Failure<Email>(Error.Validation("Email.InvalidFormat", "Định dạng email không hợp lệ."));
        }

        return Result.Success(new Email(value));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
