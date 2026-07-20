using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;

/// <summary>
/// Business Rule: Chỉ có duy nhất một chủ hộ.
/// </summary>
public sealed class HouseholdCannotHaveMultipleHeadsRule : IBusinessRule
{
    private readonly int _headCount;

    /// <summary>
    /// Khởi tạo rule với số lượng chủ hộ.
    /// </summary>
    public HouseholdCannotHaveMultipleHeadsRule(int headCount)
    {
        _headCount = headCount;
    }

    /// <inheritdoc />
    public string Message => "Chỉ có duy nhất một chủ hộ.";

    /// <inheritdoc />
    public bool IsBroken() => _headCount > 1;
}
