using System;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;

public sealed class RefreshTokenId : ValueObject
{
    public Guid Value { get; }

    public RefreshTokenId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(value));
        Value = value;
    }

    public static RefreshTokenId New() => new RefreshTokenId(Guid.NewGuid());
    public static RefreshTokenId Create(Guid value) => new RefreshTokenId(value);

    protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
