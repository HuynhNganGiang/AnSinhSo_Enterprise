using System;

namespace AnSinhSo.Contracts.Authorization;

public record RoleDto(Guid Id, string Name, string Description, bool IsSystemRole);
