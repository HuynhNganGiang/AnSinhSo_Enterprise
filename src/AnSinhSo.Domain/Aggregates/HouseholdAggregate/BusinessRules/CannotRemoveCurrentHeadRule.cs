using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;

/// <summary>
/// Business Rule: Không du?c xóa ch? h? hi?n t?i.
/// </summary>
public sealed class CannotRemoveCurrentHeadRule : IBusinessRule
{
    private readonly bool _isHead;

    /// <summary>
    /// Kh?i t?o rule.
    /// </summary>
    public CannotRemoveCurrentHeadRule(bool isHead)
    {
        _isHead = isHead;
    }

    /// <inheritdoc />
    public string Message => "Không du?c xóa ch? h? hi?n t?i.";

    /// <inheritdoc />
    public bool IsBroken() => _isHead;
}
