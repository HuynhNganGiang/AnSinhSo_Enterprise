using System;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.UserAggregate;

public sealed class RefreshToken : Entity<Guid>
{
    public string TokenHash { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public string CreatedByIp { get; private set; }
    public string? RevokedByIp { get; private set; }
    public string DeviceName { get; private set; }
    public string UserAgent { get; private set; }
    public string JwtId { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    internal RefreshToken(
        string tokenHash,
        DateTime createdAtUtc,
        DateTime expiresAtUtc,
        string createdByIp,
        string deviceName,
        string userAgent,
        string jwtId)
        : base(Guid.NewGuid())
    {
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        CreatedByIp = createdByIp;
        DeviceName = deviceName;
        UserAgent = userAgent;
        JwtId = jwtId;
    }

    private RefreshToken() : base() // ORM
    {
        TokenHash = string.Empty;
        CreatedByIp = string.Empty;
        DeviceName = string.Empty;
        UserAgent = string.Empty;
        JwtId = string.Empty;
    }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;
    public bool IsRevoked => RevokedAtUtc.HasValue;
    public bool IsActive(DateTime utcNow) => !IsRevoked && !IsExpired(utcNow);

    internal void Revoke(DateTime revokedAtUtc, string revokedByIp, string? replacedByTokenHash = null)
    {
        RevokedAtUtc = revokedAtUtc;
        RevokedByIp = revokedByIp;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}
