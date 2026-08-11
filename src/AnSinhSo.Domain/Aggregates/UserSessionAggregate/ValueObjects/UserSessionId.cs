using System;

namespace AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;

public readonly record struct UserSessionId(Guid Value)
{
    public static UserSessionId New() => new(Guid.NewGuid());
    public static UserSessionId Create(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
