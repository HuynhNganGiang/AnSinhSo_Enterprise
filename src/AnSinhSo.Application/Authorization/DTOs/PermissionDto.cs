using System;

namespace AnSinhSo.Application.Authorization.DTOs;

public record PermissionDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    Guid GroupId);
