using System.Security.Cryptography;
using AnSinhSo.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.WebUtilities;

namespace AnSinhSo.Infrastructure.Authentication;

public sealed class RefreshTokenGenerator : ITokenGenerator
{
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        RandomNumberGenerator.Fill(randomBytes);
        return WebEncoders.Base64UrlEncode(randomBytes);
    }
}
