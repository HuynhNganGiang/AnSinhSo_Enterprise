using System;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate;

public readonly struct RoleId : IEquatable<RoleId>
{
    public Guid Value { get; }

    public RoleId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("RoleId cannot be empty.", nameof(value));
        }
        Value = value;
    }

    public static RoleId Create(Guid value) => new(value);

    public bool Equals(RoleId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is RoleId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(RoleId left, RoleId right) => left.Equals(right);

    public static bool operator !=(RoleId left, RoleId right) => !(left == right);

    public override string ToString() => Value.ToString();
}
