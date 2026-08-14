using System;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate.Events;

public sealed record RoleDeletedDomainEvent(
    RoleId RoleId) : DomainEvent;

