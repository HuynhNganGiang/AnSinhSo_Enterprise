using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;

public sealed class AuditLoginId : ValueObject
{
    public Guid Value { get; }

    public AuditLoginId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(value));
        Value = value;
    }

    public static AuditLoginId New() => new AuditLoginId(Guid.NewGuid());
    public static AuditLoginId Create(Guid value) => new AuditLoginId(value);

    protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
