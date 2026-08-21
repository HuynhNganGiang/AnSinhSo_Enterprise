using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

/// <summary>
/// S? ki?n mi?n: Ð?a ch? công dân thay d?i.
/// </summary>
/// <param name="CitizenId">Ð?nh danh c?a công dân.</param>
/// <param name="Address">Ð?a ch? m?i c?a công dân.</param>
public sealed record CitizenAddressChangedDomainEvent(
    CitizenId CitizenId,
    Address Address) : DomainEvent;

