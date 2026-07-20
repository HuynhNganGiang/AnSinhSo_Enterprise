using AnSinhSo.Domain.SeedWork.Exceptions;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate.BusinessRules;

/// <summary>
/// Business Rule: Không cho Close nếu còn PaymentDetail chưa Paid.
/// </summary>
public sealed class PaymentCannotCloseWhenUnpaidRule : IBusinessRule
{
    private readonly bool _hasUnpaidDetails;

    /// <summary>
    /// Khởi tạo rule.
    /// </summary>
    public PaymentCannotCloseWhenUnpaidRule(bool hasUnpaidDetails)
    {
        _hasUnpaidDetails = hasUnpaidDetails;
    }

    /// <inheritdoc />
    public string Message => "Không cho Close nếu còn PaymentDetail chưa Paid.";

    /// <inheritdoc />
    public bool IsBroken() => _hasUnpaidDetails;
}
