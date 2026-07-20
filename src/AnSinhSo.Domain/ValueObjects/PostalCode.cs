using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho mã bưu chính.
/// </summary>
public sealed class PostalCode : ValueObject
{
    /// <summary>
    /// Giá trị mã bưu chính.
    /// </summary>
    public string Value { get; }

    private PostalCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Khởi tạo PostalCode.
    /// </summary>
    public static Result<PostalCode> Create(string value)
    {
        value = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PostalCode>(Error.Validation("PostalCode.Empty", "Mã bưu chính không được để trống."));
        }

        return Result.Success(new PostalCode(value));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
