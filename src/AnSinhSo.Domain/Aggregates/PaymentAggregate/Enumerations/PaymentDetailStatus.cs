using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate.Enumerations;

/// <summary>
/// Enumeration đại diện cho PaymentDetailStatus.
/// </summary>
public sealed class PaymentDetailStatus : Enumeration
{
    public static readonly PaymentDetailStatus Pending = new(1, nameof(Pending));
    public static readonly PaymentDetailStatus Paid = new(2, nameof(Paid));
    public static readonly PaymentDetailStatus Failed = new(3, nameof(Failed));

    private PaymentDetailStatus(int id, string name)
        : base(id, name)
    {
    }
}
