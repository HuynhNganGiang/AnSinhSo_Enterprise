using System;

namespace AnSinhSo.Application.Authorization.DTOs;

public record PermissionGroupDto(
    Guid Id,
    string Code,
    string Name,
    string Description);
