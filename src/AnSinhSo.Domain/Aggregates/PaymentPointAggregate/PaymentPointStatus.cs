using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.PaymentPointAggregate;

public sealed class PaymentPointStatus : Enumeration
{
    public static readonly PaymentPointStatus Active = new(1, nameof(Active));
    public static readonly PaymentPointStatus Inactive = new(2, nameof(Inactive));
    public static readonly PaymentPointStatus Maintenance = new(3, nameof(Maintenance));

    private PaymentPointStatus(int id, string name)
        : base(id, name)
    {
    }
}
