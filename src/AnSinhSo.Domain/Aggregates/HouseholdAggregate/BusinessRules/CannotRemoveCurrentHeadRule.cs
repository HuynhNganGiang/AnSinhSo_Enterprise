using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;

/// <summary>
/// Business Rule: Không được xóa chủ hộ hiện tại.
/// </summary>
public sealed class CannotRemoveCurrentHeadRule : IBusinessRule
{
    private readonly bool _isHead;

    /// <summary>
    /// Khởi tạo rule.
    /// </summary>
    public CannotRemoveCurrentHeadRule(bool isHead)
    {
        _isHead = isHead;
    }

    /// <inheritdoc />
    public string Message => "Không được xóa chủ hộ hiện tại.";

    /// <inheritdoc />
    public bool IsBroken() => _isHead;
}
