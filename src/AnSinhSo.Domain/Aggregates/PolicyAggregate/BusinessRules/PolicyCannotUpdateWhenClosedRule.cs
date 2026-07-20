using AnSinhSo.Domain.Aggregates.PolicyAggregate.Enumerations;
using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate.BusinessRules;

/// <summary>
/// Business Rule: Không cho Update nếu Policy đã Closed.
/// </summary>
public sealed class PolicyCannotUpdateWhenClosedRule : IBusinessRule
{
    private readonly PolicyStatus _status;

    /// <summary>
    /// Khởi tạo rule.
    /// </summary>
    public PolicyCannotUpdateWhenClosedRule(PolicyStatus status)
    {
        _status = status;
    }

    /// <inheritdoc />
    public string Message => "Không cho Update nếu Policy đã Closed.";

    /// <inheritdoc />
    public bool IsBroken() => _status == PolicyStatus.Closed;
}
