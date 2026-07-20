using System;

namespace AnSinhSo.Domain.Interfaces;

/// <summary>
/// Hợp đồng cung cấp cơ chế tạo Guid. Giúp thay thế Guid.NewGuid() cứng ngắc,
/// hỗ trợ dễ dàng kiểm thử hoặc triển khai các cơ chế Sequential Guid tùy chỉnh sau này.
/// </summary>
public interface IGuidGenerator
{
    /// <summary>
    /// Sinh ra một mã định danh Guid duy nhất mới.
    /// </summary>
    /// <returns>Một Guid ngẫu nhiên hoặc tuần tự tùy theo cài đặt.</returns>
    Guid NewGuid();
}
