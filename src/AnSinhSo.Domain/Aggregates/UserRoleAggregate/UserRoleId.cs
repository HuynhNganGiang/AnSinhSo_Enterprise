using System;

namespace AnSinhSo.Domain.Aggregates.UserRoleAggregate;

public readonly struct UserRoleId : IEquatable<UserRoleId>
{
    public Guid Value { get; }

    public UserRoleId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("UserRoleId cannot be empty.", nameof(value));
        }
        Value = value;
    }

    public static UserRoleId Create(Guid value) => new(value);

    public bool Equals(UserRoleId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is UserRoleId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(UserRoleId left, UserRoleId right) => left.Equals(right);

    public static bool operator !=(UserRoleId left, UserRoleId right) => !(left == right);

    public override string ToString() => Value.ToString();
}
