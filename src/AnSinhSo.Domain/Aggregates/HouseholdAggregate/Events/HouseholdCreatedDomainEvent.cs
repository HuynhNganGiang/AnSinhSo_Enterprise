using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

/// <summary>
/// S? ki?n mi?n: H? gia dình du?c t?o.
/// </summary>
/// <param name="HouseholdId">Ð?nh danh c?a h? gia dình du?c t?o.</param>
public sealed record HouseholdCreatedDomainEvent(
    HouseholdId HouseholdId) : DomainEvent;

