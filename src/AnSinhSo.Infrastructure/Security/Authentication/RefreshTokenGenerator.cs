using System;
using System.Security.Cryptography;
using AnSinhSo.Application.Common.Security;

namespace AnSinhSo.Infrastructure.Security.Authentication;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
