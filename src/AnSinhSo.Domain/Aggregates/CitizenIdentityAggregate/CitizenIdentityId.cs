using System;
using System.Collections.Generic;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;

public readonly struct CitizenIdentityId : IEquatable<CitizenIdentityId>
{
    public Guid Value { get; }

    public CitizenIdentityId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("CitizenIdentityId cannot be empty.", nameof(value));
        }
        Value = value;
    }

    public static CitizenIdentityId Create(Guid value) => new(value);

    public bool Equals(CitizenIdentityId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is CitizenIdentityId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(CitizenIdentityId left, CitizenIdentityId right) => left.Equals(right);

    public static bool operator !=(CitizenIdentityId left, CitizenIdentityId right) => !(left == right);

    public override string ToString() => Value.ToString();
}
