using System;
using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Entities;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Events;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;

public sealed class CitizenIdentity : AggregateRoot<CitizenIdentityId>
{
    private readonly List<LinkedProvider> _linkedProviders = new();
    private readonly List<TrustedDevice> _trustedDevices = new();

    public CitizenId CitizenId { get; private set; }
    public PhoneNumber? PrimaryPhone { get; private set; }
    public IdentityStatus Status { get; private set; }
    public string SecurityStamp { get; private set; } = string.Empty;
    public int FailedAttemptCount { get; private set; }

    public IReadOnlyCollection<LinkedProvider> LinkedProviders => _linkedProviders.AsReadOnly();
    public IReadOnlyCollection<TrustedDevice> TrustedDevices => _trustedDevices.AsReadOnly();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private CitizenIdentity() { } // ORM
#pragma warning restore CS8618

    /// <summary>
    /// Factory Method - Pure Domain. 
    /// Tham số được inject từ Application Layer, bảo đảm tính duy nhất của CitizenId phải được check ở tầng ngoài (AD #25).
    /// </summary>
    public static CitizenIdentity Create(
        CitizenIdentityId id, 
        CitizenId citizenId, 
        string securityStamp,
        PhoneNumber? primaryPhone = null)
    {
        var identity = new CitizenIdentity
        {
            Id = id,
            CitizenId = citizenId,
            Status = IdentityStatus.Pending,
            SecurityStamp = securityStamp,
            PrimaryPhone = primaryPhone,
            FailedAttemptCount = 0
        };

        // Note: Creation event omitted here to keep it pure, usually raised by Application Layer or here if createdAt was passed.
        return identity;
    }

    public void VerifyPhoneNumber(PhoneNumber phoneNumber, DateTime verifiedAt)
    {
        // AD #26: Only from PendingVerification
        if (Status != IdentityStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot verify phone number when status is {Status}. Must be Pending.");
        }

        PrimaryPhone = phoneNumber;
        Status = IdentityStatus.Verified;

        AddDomainEvent(new CitizenIdentityActivatedDomainEvent(Id, CitizenId, verifiedAt));
    }

    public void LinkExternalProvider(Guid providerEntityId, ProviderType type, string subjectId, DateTime linkedAt)
    {
        // AD #26: If Locked or Suspended, block linking.
        if (Status == IdentityStatus.Suspended || Status == IdentityStatus.Suspended)
        {
            throw new InvalidOperationException($"Cannot link provider when identity is {Status}.");
        }

        if (_linkedProviders.Any(p => p.ProviderType == type))
        {
            throw new InvalidOperationException($"A provider of type {type} is already linked.");
        }

        var provider = new LinkedProvider(providerEntityId, type, subjectId, linkedAt);
        _linkedProviders.Add(provider);
        
        AddDomainEvent(new ExternalProviderLinkedDomainEvent(Id, type, linkedAt));

        // AD #26: Auto-activate if pending
        if (Status == IdentityStatus.Pending)
        {
            Status = IdentityStatus.Verified;
            AddDomainEvent(new CitizenIdentityActivatedDomainEvent(Id, CitizenId, linkedAt));
        }
    }

    public void AddTrustedDevice(Guid deviceEntityId, string deviceId, string deviceName, DateTime trustedAt, int maxDevices)
    {
        // AD #28: Trust Device Policy
        if (_trustedDevices.Count >= maxDevices)
        {
            throw new InvalidOperationException($"Maximum trusted devices ({maxDevices}) reached.");
        }

        if (_trustedDevices.Any(d => d.DeviceId == deviceId))
        {
            throw new InvalidOperationException("Device is already trusted.");
        }

        var device = new TrustedDevice(deviceEntityId, deviceId, deviceName, trustedAt);
        _trustedDevices.Add(device);
    }

    public void RecordFailedAttempt(int maxAttempts, string newSecurityStamp, DateTime failedAt)
    {
        if (Status == IdentityStatus.Suspended || Status == IdentityStatus.Suspended)
        {
            return; // Already locked/suspended
        }

        FailedAttemptCount++;

        if (FailedAttemptCount >= maxAttempts)
        {
            Status = IdentityStatus.Suspended;
            SecurityStamp = newSecurityStamp;
            AddDomainEvent(new CitizenIdentityLockedDomainEvent(Id, "Exceeded maximum failed attempts.", failedAt));
        }
    }

    public void ResetFailedAttempts()
    {
        FailedAttemptCount = 0;
    }
}
