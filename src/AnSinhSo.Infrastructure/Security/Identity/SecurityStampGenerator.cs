using System;
using System.Security.Cryptography;
using AnSinhSo.Application.Common.Interfaces.Security;

namespace AnSinhSo.Infrastructure.Security.Identity;

public class SecurityStampGenerator : ISecurityStampGenerator
{
    public string Generate()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
