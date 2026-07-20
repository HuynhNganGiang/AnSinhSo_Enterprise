using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;

/// <summary>
/// Business Rule: Không được thêm CitizenId đã tồn tại.
/// </summary>
public sealed class CannotAddDuplicateCitizenRule : IBusinessRule
{
    private readonly bool _alreadyExists;

    /// <summary>
    /// Khởi tạo rule.
    /// </summary>
    public CannotAddDuplicateCitizenRule(bool alreadyExists)
    {
        _alreadyExists = alreadyExists;
    }

    /// <inheritdoc />
    public string Message => "Không được thêm CitizenId đã tồn tại.";

    /// <inheritdoc />
    public bool IsBroken() => _alreadyExists;
}
