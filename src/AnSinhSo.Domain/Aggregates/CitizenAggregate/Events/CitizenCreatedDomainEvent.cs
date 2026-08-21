using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

/// <summary>
/// S? ki?n mi?n: Công dân du?c t?o.
/// </summary>
/// <param name="CitizenId">Ð?nh danh c?a công dân du?c t?o.</param>
public sealed record CitizenCreatedDomainEvent(
    CitizenId CitizenId) : DomainEvent;

