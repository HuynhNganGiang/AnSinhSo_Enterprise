using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

/// <summary>
/// S? ki?n mi?n: S? di?n tho?i công dân thay d?i.
/// </summary>
/// <param name="CitizenId">Ð?nh danh c?a công dân.</param>
/// <param name="PhoneNumber">S? di?n tho?i m?i c?a công dân.</param>
public sealed record CitizenPhoneChangedDomainEvent(
    CitizenId CitizenId,
    PhoneNumber PhoneNumber) : DomainEvent;

