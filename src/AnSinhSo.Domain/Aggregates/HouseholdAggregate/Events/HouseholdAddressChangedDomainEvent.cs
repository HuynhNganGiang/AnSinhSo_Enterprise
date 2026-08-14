using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

public sealed record HouseholdAddressChangedDomainEvent(HouseholdId HouseholdId) : DomainEvent;
