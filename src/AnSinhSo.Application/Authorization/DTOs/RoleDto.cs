using System;

namespace AnSinhSo.Application.Authorization.DTOs;

public record RoleDto(
    Guid Id,
    string Name,
    string Description,
    bool IsSystemRole);
