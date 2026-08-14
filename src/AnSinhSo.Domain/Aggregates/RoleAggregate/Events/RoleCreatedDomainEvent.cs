using System;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate.Events;

public sealed record RoleCreatedDomainEvent(
    RoleId RoleId,
    string Name,
    bool IsSystemRole) : DomainEvent;

