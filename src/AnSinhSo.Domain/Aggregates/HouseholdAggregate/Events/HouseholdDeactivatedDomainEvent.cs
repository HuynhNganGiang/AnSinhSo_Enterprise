using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

public sealed record HouseholdDeactivatedDomainEvent(HouseholdId HouseholdId) : DomainEvent;
