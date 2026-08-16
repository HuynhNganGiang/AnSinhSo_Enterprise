using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;

public sealed record CitizenLocationUpdatedDomainEvent(CitizenId CitizenId, Location Location) : IDomainEvent;
