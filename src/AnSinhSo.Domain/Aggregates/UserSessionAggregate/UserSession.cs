using System;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.UserSessionAggregate;

public sealed class UserSession : AggregateRoot<UserSessionId>
{
    public Guid CitizenIdentityId { get; private set; }
    public Guid RefreshTokenFamilyId { get; private set; }
    public DeviceInfo DeviceInfo { get; private set; }
    public string RefreshTokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string RevokeReason { get; private set; }

#pragma warning disable CS8618
    private UserSession() { }
#pragma warning restore CS8618

    private UserSession(
        UserSessionId id,
        Guid citizenIdentityId,
        Guid refreshTokenFamilyId,
        DeviceInfo deviceInfo,
        string refreshTokenHash,
        DateTime expiresAt)
    {
        Id = id;
        CitizenIdentityId = citizenIdentityId;
        RefreshTokenFamilyId = refreshTokenFamilyId;
        DeviceInfo = deviceInfo ?? throw new ArgumentNullException(nameof(deviceInfo));
        RefreshTokenHash = refreshTokenHash ?? throw new ArgumentNullException(nameof(refreshTokenHash));
        ExpiresAt = expiresAt;
        
        IsRevoked = false;
        RevokedAt = null;
        RevokeReason = string.Empty;
    }

    public static UserSession Create(
        Guid citizenIdentityId,
        Guid refreshTokenFamilyId,
        DeviceInfo deviceInfo,
        string refreshTokenHash,
        DateTime expiresAt)
    {
        return new UserSession(
            UserSessionId.New(),
            citizenIdentityId,
            refreshTokenFamilyId,
            deviceInfo,
            refreshTokenHash,
            expiresAt);
    }

    public void RotateRefreshToken(string newRefreshTokenHash, DateTime expiresAt)
    {
        if (IsRevoked)
            throw new InvalidOperationException("Cannot rotate refresh token for a revoked session.");
            
        if (IsExpired())
            throw new InvalidOperationException("Cannot rotate refresh token for an expired session.");

        RefreshTokenHash = newRefreshTokenHash ?? throw new ArgumentNullException(nameof(newRefreshTokenHash));
        ExpiresAt = expiresAt;
    }

    public void Revoke(string reason)
    {
        if (IsRevoked)
            return; // Already revoked

        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokeReason = reason ?? "Unspecified";
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }

    public bool IsActive()
    {
        return !IsRevoked && !IsExpired();
    }
}
