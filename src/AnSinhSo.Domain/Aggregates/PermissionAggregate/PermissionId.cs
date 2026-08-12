using System;

namespace AnSinhSo.Domain.Aggregates.PermissionAggregate;

public readonly struct PermissionId : IEquatable<PermissionId>
{
    public Guid Value { get; }

    public PermissionId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("PermissionId cannot be empty.", nameof(value));
        }
        Value = value;
    }

    public static PermissionId Create(Guid value) => new(value);

    public bool Equals(PermissionId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is PermissionId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(PermissionId left, PermissionId right) => left.Equals(right);

    public static bool operator !=(PermissionId left, PermissionId right) => !(left == right);

    public override string ToString() => Value.ToString();
}
