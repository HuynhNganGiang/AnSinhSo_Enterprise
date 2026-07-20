using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

/// <summary>
/// Sự kiện miền: Số điện thoại công dân thay đổi.
/// </summary>
/// <param name="CitizenId">Định danh của công dân.</param>
/// <param name="PhoneNumber">Số điện thoại mới của công dân.</param>
public sealed record CitizenPhoneChangedDomainEvent(
    CitizenId CitizenId,
    PhoneNumber PhoneNumber) : DomainEvent;

