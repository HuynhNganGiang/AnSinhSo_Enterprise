using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate.Enumerations;

/// <summary>
/// Enumeration đại diện cho PaymentStatus.
/// </summary>
public sealed class PaymentStatus : Enumeration
{
    public static readonly PaymentStatus Open = new(1, nameof(Open));
    public static readonly PaymentStatus Closed = new(2, nameof(Closed));
    public static readonly PaymentStatus Cancelled = new(3, nameof(Cancelled));

    private PaymentStatus(int id, string name)
        : base(id, name)
    {
    }
}
