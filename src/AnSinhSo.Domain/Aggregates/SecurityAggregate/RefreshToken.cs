using System;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate;

public sealed class RefreshToken : AggregateRoot<RefreshTokenId>
{
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; }
    public Guid FamilyId { get; private set; }
    
    public DateTime ExpiredAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string RevokedReason { get; private set; }
    public DeviceSessionId DeviceSessionId { get; private set; }

    public bool IsRevoked => RevokedAt != null;
    public bool IsExpired => DateTime.UtcNow >= ExpiredAt;
    public bool IsActive => !IsRevoked && !IsExpired;

#pragma warning disable CS8618
    private RefreshToken() { }
#pragma warning restore CS8618

    private RefreshToken(RefreshTokenId id, Guid userId, string tokenHash, Guid familyId, DateTime expiredAt, DeviceSessionId deviceSessionId)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        FamilyId = familyId;
        CreatedAt = DateTime.UtcNow;
        ExpiredAt = expiredAt;
        DeviceSessionId = deviceSessionId;
        RevokedReason = string.Empty;
    }

    public static RefreshToken Create(Guid userId, string tokenHash, Guid familyId, DateTime expiredAt, DeviceSessionId deviceSessionId)
    {
        return new RefreshToken(RefreshTokenId.New(), userId, tokenHash, familyId, expiredAt, deviceSessionId);
    }

    public void Revoke(string reason)
    {
        if (IsRevoked) return;
        RevokedAt = DateTime.UtcNow;
        RevokedReason = reason;
    }
}
