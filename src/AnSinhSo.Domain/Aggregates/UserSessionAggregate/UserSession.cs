using System;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.UserSessionAggregate;

public sealed class UserSession : AggregateRoot<UserSessionId>
{
    public UserId UserId { get; private set; }
    public Guid FamilyId { get; private set; }
    public string CurrentTokenHash { get; private set; } = string.Empty;
    public string SecurityStampSnapshot { get; private set; } = string.Empty;
    public DeviceMetadata Metadata { get; private set; } = null!;
    

    public DateTime LastActivityUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    
    public DateTime? RevokedAtUtc { get; private set; }
    public string? RevokedReason { get; private set; }
    public bool IsRevoked { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

#pragma warning disable CS8618
    private UserSession() { } // ORM
#pragma warning restore CS8618

    public static UserSession Issue(
        Guid userId,
        string tokenHash,
        DateTime expiresAtUtc,
        string securityStamp,
        string ipAddress,
        string deviceName,
        string userAgent,
        DateTime now)
    {
        return new UserSession
        {
            Id = UserSessionId.New(),
            UserId = new UserId(userId),
            FamilyId = Guid.NewGuid(),
            CurrentTokenHash = tokenHash,
            SecurityStampSnapshot = securityStamp,
            Metadata = new DeviceMetadata(deviceName, ipAddress, userAgent),

            LastActivityUtc = now,
            ExpiresAtUtc = expiresAtUtc,
            IsRevoked = false
        };
    }
}
