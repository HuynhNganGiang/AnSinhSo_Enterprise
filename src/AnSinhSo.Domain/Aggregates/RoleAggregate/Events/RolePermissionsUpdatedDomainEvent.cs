using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.Aggregates.PermissionAggregate;

namespace AnSinhSo.Domain.Aggregates.RoleAggregate.Events;

public sealed record RolePermissionsUpdatedDomainEvent(
    RoleId RoleId,
    IReadOnlyCollection<PermissionId> AddedPermissions,
    IReadOnlyCollection<PermissionId> RemovedPermissions) : DomainEvent;
