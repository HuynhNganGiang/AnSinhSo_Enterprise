using System;

namespace AnSinhSo.Domain.SeedWork.Results;

/// <summary>
/// Đại diện cho một lỗi nghiệp vụ trong hệ thống.
/// </summary>
public sealed record Error(string Code, string Message, ErrorType Type)
{
    /// <summary>
    /// Giá trị mặc định cho lỗi rỗng (không có lỗi).
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    /// <summary>
    /// Giá trị mặc định khi có lỗi Null.
    /// </summary>
    public static readonly Error NullValue = new("General.Null", "Giá trị bị rỗng.", ErrorType.Failure);

    /// <summary>
    /// Khởi tạo Error dạng NotFound.
    /// </summary>
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    /// <summary>
    /// Khởi tạo Error dạng Validation.
    /// </summary>
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    /// <summary>
    /// Khởi tạo Error dạng Conflict.
    /// </summary>
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    /// <summary>
    /// Khởi tạo Error dạng Failure mặc định.
    /// </summary>
    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    /// <summary>
    /// Khởi tạo Error dạng TooManyRequests.
    /// </summary>
    public static Error TooManyRequests(string code, string message) => new(code, message, ErrorType.TooManyRequests);
}
