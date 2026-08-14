using System;
using System.Collections.Generic;

namespace AnSinhSo.Application.Authorization.DTOs;

public record PermissionGroupWithPermissionsDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    IReadOnlyCollection<PermissionDto> Permissions);
