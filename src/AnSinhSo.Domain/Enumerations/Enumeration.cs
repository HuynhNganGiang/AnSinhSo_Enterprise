using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnSinhSo.Domain.Enumerations;

/// <summary>
/// Lớp cơ sở cho mô hình Enumeration Pattern theo DDD.
/// Tránh sử dụng Enum nguyên thủy cho những loại có khả năng mở rộng.
/// </summary>
public abstract class Enumeration : IComparable
{
    /// <summary>
    /// Định danh của Enumeration.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Tên hiển thị hoặc giá trị của Enumeration.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Khởi tạo một đối tượng Enumeration.
    /// </summary>
    /// <param name="id">Mã định danh.</param>
    /// <param name="name">Tên.</param>
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Lấy danh sách tất cả các giá trị của một kiểu Enumeration cụ thể.
    /// </summary>
    /// <typeparam name="T">Kiểu cụ thể kế thừa từ Enumeration.</typeparam>
    /// <returns>Danh sách các đối tượng Enumeration.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    /// <inheritdoc/>
    public int CompareTo(object? other)
    {
        if (other is not Enumeration enumeration)
            throw new ArgumentException($"Tối tượng phải là kiểu {nameof(Enumeration)}", nameof(other));

        return Id.CompareTo(enumeration.Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
