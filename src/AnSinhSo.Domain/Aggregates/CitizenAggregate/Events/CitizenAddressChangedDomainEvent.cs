using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

/// <summary>
/// Sự kiện miền: Địa chỉ công dân thay đổi.
/// </summary>
/// <param name="CitizenId">Định danh của công dân.</param>
/// <param name="Address">Địa chỉ mới của công dân.</param>
public sealed record CitizenAddressChangedDomainEvent(
    CitizenId CitizenId,
    Address Address) : DomainEvent;

