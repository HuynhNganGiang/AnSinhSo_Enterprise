using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;

public sealed class DeviceSessionId : ValueObject
{
    public Guid Value { get; }

    public DeviceSessionId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(value));
        Value = value;
    }

    public static DeviceSessionId New() => new DeviceSessionId(Guid.NewGuid());
    public static DeviceSessionId Create(Guid value) => new DeviceSessionId(value);

    protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
