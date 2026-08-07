using System;

namespace AnSinhSo.Application.Common.Security;

public sealed record TokenResult(
    string AccessToken,
    DateTime ExpiresAtUtc,
    string JwtId,
    int RefreshTokenDays
);
