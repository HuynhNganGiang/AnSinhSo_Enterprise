namespace AnSinhSo.Domain.SeedWork.Results;

/// <summary>
/// Các loại lỗi nghiệp vụ của hệ thống.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Không có lỗi.
    /// </summary>
    None = 0,

    /// <summary>
    /// Thất bại chung của miền (Generic Domain Error).
    /// </summary>
    Failure = 1,

    /// <summary>
    /// Lỗi do dữ liệu đầu vào không hợp lệ.
    /// </summary>
    Validation = 2,

    /// <summary>
    /// Lỗi không tìm thấy đối tượng.
    /// </summary>
    NotFound = 3,

    /// <summary>
    /// Lỗi xung đột trạng thái.
    /// </summary>
    Conflict = 4,

    /// <summary>
    /// Lỗi vi phạm phân quyền/truy cập.
    /// </summary>
    Forbidden = 5,

    /// <summary>
    /// Lỗi liên quan đến xác thực.
    /// </summary>
    Unauthorized = 6,

    /// <summary>
    /// Yêu cầu quá nhiều lần.
    /// </summary>
    TooManyRequests = 7
}
