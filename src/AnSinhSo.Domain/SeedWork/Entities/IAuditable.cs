using System;

namespace AnSinhSo.Domain.SeedWork.Entities;

/// <summary>
/// Giao diện quy định các thông tin theo vết (Audit) của một thực thể.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Thời điểm thực thể được tạo.
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Định danh người tạo.
    /// </summary>
    string? CreatedBy { get; }

    /// <summary>
    /// Thời điểm thực thể được cập nhật lần cuối.
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Định danh người cập nhật lần cuối.
    /// </summary>
    string? UpdatedBy { get; }
}
