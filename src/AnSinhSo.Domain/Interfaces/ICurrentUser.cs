using System.Collections.Generic;

namespace AnSinhSo.Domain.Interfaces;

/// <summary>
/// Hợp đồng cung cấp thông tin ngữ cảnh người dùng đang thực hiện yêu cầu (Request).
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Định danh của người dùng (nếu có).
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Tên tài khoản hoặc tên hiển thị của người dùng (nếu có).
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Danh sách các quyền/vai trò của người dùng trong phiên làm việc hiện tại.
    /// </summary>
    IReadOnlyList<string> Roles { get; }

    /// <summary>
    /// Cờ xác định xem người dùng đã được xác thực thành công hay chưa.
    /// </summary>
    bool IsAuthenticated { get; }
}
