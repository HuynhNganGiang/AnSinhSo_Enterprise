using System;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

public readonly record struct PaymentId
{
    public Guid Value { get; }

    public PaymentId(Guid value)
    {
        Value = value;
    }

    public static PaymentId New() => new(Guid.NewGuid());
    public static PaymentId Empty => new(Guid.Empty);

    public override string ToString() => Value.ToString();
}
