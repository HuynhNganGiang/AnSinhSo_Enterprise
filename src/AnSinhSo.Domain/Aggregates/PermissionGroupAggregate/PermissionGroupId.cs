using System;

namespace AnSinhSo.Domain.Aggregates.PermissionGroupAggregate;

public readonly struct PermissionGroupId : IEquatable<PermissionGroupId>
{
    public Guid Value { get; }

    public PermissionGroupId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("PermissionGroupId cannot be empty.", nameof(value));
        }
        Value = value;
    }

    public static PermissionGroupId Create(Guid value) => new(value);

    public bool Equals(PermissionGroupId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is PermissionGroupId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(PermissionGroupId left, PermissionGroupId right) => left.Equals(right);

    public static bool operator !=(PermissionGroupId left, PermissionGroupId right) => !(left == right);

    public override string ToString() => Value.ToString();
}
