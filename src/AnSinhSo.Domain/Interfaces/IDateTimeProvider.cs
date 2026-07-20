using System;

namespace AnSinhSo.Domain.Interfaces;

/// <summary>
/// Hợp đồng cung cấp thông tin ngày giờ hệ thống, dùng để trừu tượng hóa DateTime.UtcNow.
/// Giúp dễ dàng Unit Test các logic liên quan đến thời gian ở tầng Domain.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Trả về thời gian chuẩn UTC tại thời điểm gọi.
    /// </summary>
    DateTime UtcNow { get; }
}
