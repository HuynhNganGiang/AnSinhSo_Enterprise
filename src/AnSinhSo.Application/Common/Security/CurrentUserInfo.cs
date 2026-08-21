using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AnSinhSo.Application.Common.Security;

public sealed record CurrentUserInfo(
    Guid UserId,
    string Username,
    string Email,
    IReadOnlyList<string> Roles,
    bool IsAuthenticated,
    ClaimsPrincipal ClaimsPrincipal
);
