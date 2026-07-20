using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

/// <summary>
/// Sự kiện miền: Công dân được tạo.
/// </summary>
/// <param name="CitizenId">Định danh của công dân được tạo.</param>
public sealed record CitizenCreatedDomainEvent(
    CitizenId CitizenId) : DomainEvent;

