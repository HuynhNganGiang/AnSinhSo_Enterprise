using System;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate;

public sealed class DeviceSession : AggregateRoot<DeviceSessionId>
{
    public Guid UserId { get; private set; }
    public string DeviceName { get; private set; }
    public string Browser { get; private set; }
    public string OS { get; private set; }
    public string Platform { get; private set; }
    public string IPAddress { get; private set; }
    public string Fingerprint { get; private set; }
    public Guid? RefreshTokenId { get; private set; }
    public DateTime LastSeenAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? Reason { get; private set; }
    public bool IsTrusted { get; private set; }
    public string SecurityStamp { get; private set; } = string.Empty;
    public byte[] RowVersion { get; private set; } = default!;
    public bool IsArchived { get; private set; }

#pragma warning disable CS8618
    private DeviceSession() { }
#pragma warning restore CS8618

    private DeviceSession(DeviceSessionId id, Guid userId, string deviceName, string browser, string os, string platform, string ipAddress, string fingerprint, bool isTrusted, string securityStamp)
    {
        Id = id;
        UserId = userId;
        DeviceName = deviceName;
        Browser = browser;
        OS = os;
        Platform = platform;
        IPAddress = ipAddress;
        Fingerprint = fingerprint;
        IsTrusted = isTrusted;
        SecurityStamp = securityStamp;
        LastSeenAt = DateTime.UtcNow;
    }

    public static DeviceSession Create(Guid userId, string deviceName, string browser, string os, string platform, string ipAddress, string fingerprint, bool isTrusted, string securityStamp)
    {
        return new DeviceSession(DeviceSessionId.New(), userId, deviceName, browser, os, platform, ipAddress, fingerprint, isTrusted, securityStamp);
    }

    public void UpdateLastSeen(string ipAddress)
    {
        IPAddress = ipAddress;
        LastSeenAt = DateTime.UtcNow;
    }

    public void LinkRefreshToken(Guid refreshTokenId)
    {
        RefreshTokenId = refreshTokenId;
    }

    public void UpdateLastSeen()
    {
        LastSeenAt = DateTime.UtcNow;
    }

    public void Revoke(string reason)
    {
        RevokedAt = DateTime.UtcNow;
        Reason = reason;
    }

    public bool IsActive()
    {
        return RevokedAt == null && !IsArchived;
    }

    public void Archive()
    {
        IsArchived = true;
    }
}
