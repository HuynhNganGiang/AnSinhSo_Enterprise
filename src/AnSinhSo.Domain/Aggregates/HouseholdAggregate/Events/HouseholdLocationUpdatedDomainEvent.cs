using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

public sealed record HouseholdLocationUpdatedDomainEvent(HouseholdId HouseholdId, Location Location) : IDomainEvent;
