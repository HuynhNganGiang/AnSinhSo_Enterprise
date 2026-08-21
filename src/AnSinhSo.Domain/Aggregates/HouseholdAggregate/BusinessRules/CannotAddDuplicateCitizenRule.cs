using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;

/// <summary>
/// Business Rule: Không du?c thêm CitizenId dã t?n t?i.
/// </summary>
public sealed class CannotAddDuplicateCitizenRule : IBusinessRule
{
    private readonly bool _alreadyExists;

    /// <summary>
    /// Kh?i t?o rule.
    /// </summary>
    public CannotAddDuplicateCitizenRule(bool alreadyExists)
    {
        _alreadyExists = alreadyExists;
    }

    /// <inheritdoc />
    public string Message => "Không du?c thêm CitizenId dã t?n t?i.";

    /// <inheritdoc />
    public bool IsBroken() => _alreadyExists;
}
