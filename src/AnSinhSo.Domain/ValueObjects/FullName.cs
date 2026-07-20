using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho họ tên đầy đủ.
/// </summary>
public sealed class FullName : ValueObject
{
    /// <summary>
    /// Họ.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Tên đệm.
    /// </summary>
    public string MiddleName { get; }

    /// <summary>
    /// Tên.
    /// </summary>
    public string LastName { get; }

    private FullName(string firstName, string middleName, string lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    /// <summary>
    /// Khởi tạo FullName.
    /// </summary>
    public static Result<FullName> Create(string firstName, string middleName, string lastName)
    {
        firstName = firstName?.Trim() ?? string.Empty;
        middleName = middleName?.Trim() ?? string.Empty;
        lastName = lastName?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FullName>(Error.Validation("FullName.FirstNameEmpty", "Họ không được để trống."));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<FullName>(Error.Validation("FullName.LastNameEmpty", "Tên không được để trống."));
        }

        return Result.Success(new FullName(firstName, middleName ?? string.Empty, lastName));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return MiddleName;
        yield return LastName;
    }
}
