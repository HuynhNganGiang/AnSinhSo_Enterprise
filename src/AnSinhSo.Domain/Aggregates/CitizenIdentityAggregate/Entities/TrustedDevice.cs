using System;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Entities;

public sealed class TrustedDevice : Entity<Guid>
{
    public string DeviceId { get; private set; } = string.Empty;
    public string DeviceName { get; private set; } = string.Empty;
    public DateTime TrustedAt { get; private set; }
    public DateTime LastUsedAt { get; private set; }

    private TrustedDevice() { } // ORM

    internal TrustedDevice(Guid id, string deviceId, string deviceName, DateTime trustedAt)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("DeviceId cannot be empty.", nameof(deviceId));
        }

        DeviceId = deviceId;
        DeviceName = deviceName ?? "Unknown Device";
        TrustedAt = trustedAt;
        LastUsedAt = trustedAt;
    }

    internal void RecordUsage(DateTime usedAt)
    {
        LastUsedAt = usedAt;
    }
}
