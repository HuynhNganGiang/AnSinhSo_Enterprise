using System;

namespace AnSinhSo.Domain.SeedWork.Events;

/// <summary>
/// Lớp cơ sở cho các sự kiện miền, ghi nhận thời điểm và định danh của sự kiện.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh duy nhất của sự kiện.
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Thời điểm sự kiện xảy ra tính theo giờ chuẩn quốc tế (UTC).
    /// </summary>
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}
