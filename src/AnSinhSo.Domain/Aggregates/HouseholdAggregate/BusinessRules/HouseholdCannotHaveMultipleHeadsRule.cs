using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;

/// <summary>
/// Business Rule: Ch? có duy nh?t m?t ch? h?.
/// </summary>
public sealed class HouseholdCannotHaveMultipleHeadsRule : IBusinessRule
{
    private readonly int _headCount;

    /// <summary>
    /// Kh?i t?o rule v?i s? lu?ng ch? h?.
    /// </summary>
    public HouseholdCannotHaveMultipleHeadsRule(int headCount)
    {
        _headCount = headCount;
    }

    /// <inheritdoc />
    public string Message => "Ch? có duy nh?t m?t ch? h?.";

    /// <inheritdoc />
    public bool IsBroken() => _headCount > 1;
}
