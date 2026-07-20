using System.Collections.Generic;

namespace AnSinhSo.Domain.SeedWork.Events;

/// <summary>
/// Giao diện yêu cầu thực thể phải có khả năng lưu trữ sự kiện miền.
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Lấy danh sách các sự kiện miền chưa được xuất bản.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();

    /// <summary>
    /// Thêm một sự kiện miền vào thực thể.
    /// </summary>
    /// <param name="domainEvent">Sự kiện miền cần thêm.</param>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Xóa toàn bộ các sự kiện miền sau khi đã xuất bản.
    /// </summary>
    void ClearDomainEvents();
}
