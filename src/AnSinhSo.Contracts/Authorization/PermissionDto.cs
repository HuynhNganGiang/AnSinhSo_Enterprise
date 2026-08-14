using System;

namespace AnSinhSo.Contracts.Authorization;

public record PermissionDto(Guid Id, string Code, string Name, string Description, Guid GroupId);
