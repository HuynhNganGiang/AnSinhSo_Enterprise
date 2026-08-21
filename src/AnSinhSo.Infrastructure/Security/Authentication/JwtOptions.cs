using System.ComponentModel.DataAnnotations;

namespace AnSinhSo.Infrastructure.Security.Authentication;

public sealed class JwtOptions
{
    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Required]
    public string SecretKey { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenMinutes { get; init; }

    [Range(1, 365)]
    public int RefreshTokenDays { get; init; }

    public int ClockSkewSeconds { get; init; }

    [Required]
    public string SigningAlgorithm { get; init; } = string.Empty;
}
