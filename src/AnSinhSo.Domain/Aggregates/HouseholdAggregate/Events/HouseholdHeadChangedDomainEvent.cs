using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;

public sealed record HouseholdHeadChangedDomainEvent(HouseholdId HouseholdId, CitizenId NewHeadCitizenId) : DomainEvent;
