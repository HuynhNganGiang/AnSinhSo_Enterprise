using System;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Events;

public record CitizenLoggedInEvent(
    Guid CitizenIdentityId,
    Guid CitizenId,
    Guid UserId,
    Guid DeviceSessionId,
    string IpAddress,
    DateTime OccurredOn
) : DomainEvent;
