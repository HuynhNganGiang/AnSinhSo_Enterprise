using System;
using System.Collections.Generic;

namespace AnSinhSo.Contracts.Authorization;

public record UpdateRolePermissionsRequest(List<Guid> PermissionIds);
