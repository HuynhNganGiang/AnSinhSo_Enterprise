using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;

public sealed class LoginHistoryId : ValueObject
{
    public Guid Value { get; }

    public LoginHistoryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(value));
        Value = value;
    }

    public static LoginHistoryId New() => new LoginHistoryId(Guid.NewGuid());
    public static LoginHistoryId Create(Guid value) => new LoginHistoryId(value);

    protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
