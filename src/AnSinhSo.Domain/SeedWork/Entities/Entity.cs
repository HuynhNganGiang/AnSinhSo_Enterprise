using System;
using System.Collections.Generic;

namespace AnSinhSo.Domain.SeedWork.Entities;

/// <summary>
/// Lớp cơ sở cho thực thể có định danh.
/// </summary>
/// <typeparam name="TId">Kiểu dữ liệu của định danh.</typeparam>
public abstract class Entity<TId> : BaseEntity, IEquatable<Entity<TId>>
{
    /// <summary>
    /// Định danh duy nhất của thực thể.
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Hàm khởi tạo bảo vệ.
    /// </summary>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Hàm khởi tạo mặc định cho ORM.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Entity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        if (obj is not Entity<TId> other)
            return false;

        return Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (EqualityComparer<TId>.Default.Equals(Id, default) ||
            EqualityComparer<TId>.Default.Equals(other.Id, default))
        {
            return false;
        }

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc/>
    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    /// <inheritdoc/>
    public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
    {
        return !(a == b);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }
}
