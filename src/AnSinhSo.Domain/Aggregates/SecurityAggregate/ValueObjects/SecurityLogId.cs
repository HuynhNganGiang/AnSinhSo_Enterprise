using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;

public sealed class SecurityLogId : ValueObject
{
    public Guid Value { get; }

    public SecurityLogId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(value));
        Value = value;
    }

    public static SecurityLogId New() => new SecurityLogId(Guid.NewGuid());
    public static SecurityLogId Create(Guid value) => new SecurityLogId(value);

    protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
