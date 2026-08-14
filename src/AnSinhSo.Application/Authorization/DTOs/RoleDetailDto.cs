using System;
using System.Collections.Generic;

namespace AnSinhSo.Application.Authorization.DTOs;

public record RoleDetailDto(
    Guid Id,
    string Name,
    string Description,
    bool IsSystemRole,
    IReadOnlyCollection<PermissionDto> Permissions);
