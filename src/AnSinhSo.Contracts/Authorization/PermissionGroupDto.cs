using System;
using System.Collections.Generic;

namespace AnSinhSo.Contracts.Authorization;

public record PermissionGroupDto(Guid Id, string Name, string Description, List<PermissionDto> Permissions);
