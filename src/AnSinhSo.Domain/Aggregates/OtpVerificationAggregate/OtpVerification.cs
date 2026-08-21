using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Events;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;

public sealed class OtpVerification : AggregateRoot<OtpVerificationId>
{
    public CitizenIdentityId CitizenIdentityId { get; private set; }
    public Guid RequestId { get; private set; }
    public string CodeHash { get; private set; } = string.Empty;
    public PhoneNumber TargetPhone { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public int FailedAttemptCount { get; private set; }
    public OtpStatus Status { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private OtpVerification() { } // ORM
#pragma warning restore CS8618

    private OtpVerification(
        OtpVerificationId id,
        CitizenIdentityId citizenIdentityId,
        Guid requestId,
        string codeHash,
        PhoneNumber targetPhone,
        DateTime expiresAt)
    {
        Id = id;
        CitizenIdentityId = citizenIdentityId;
        RequestId = requestId;
        CodeHash = codeHash;
        TargetPhone = targetPhone;
        ExpiresAt = expiresAt;
        Status = OtpStatus.Pending;
        FailedAttemptCount = 0;
    }

    public static OtpVerification Create(
        CitizenIdentityId citizenIdentityId,
        Guid requestId,
        string codeHash,
        PhoneNumber targetPhone,
        DateTime expiresAt)
    {
        return new OtpVerification(
            OtpVerificationId.New(),
            citizenIdentityId,
            requestId,
            codeHash,
            targetPhone,
            expiresAt);
    }

    public void Verify(string providedCodeHash, int maxAttempts, DateTime now)
    {
        // AD #56: OTP One-Time Usage
        if (Status == OtpStatus.Verified || Status == OtpStatus.Used)
        {
            throw new InvalidOperationException("This OTP has already been verified/used.");
        }

        if (Status != OtpStatus.Pending)
        {
            throw new InvalidOperationException($"OTP cannot be verified. Status is {Status}.");
        }

        if (now > ExpiresAt)
        {
            Status = OtpStatus.Expired;
            throw new InvalidOperationException("OTP has expired.");
        }

        if (FailedAttemptCount >= maxAttempts)
        {
            Status = OtpStatus.Locked;
            throw new InvalidOperationException("OTP locked due to too many failed attempts.");
        }

        if (CodeHash != providedCodeHash)
        {
            FailedAttemptCount++;
            if (FailedAttemptCount >= maxAttempts)
            {
                Status = OtpStatus.Locked;
            }
            throw new InvalidOperationException("Invalid OTP code.");
        }

        Status = OtpStatus.Verified;
        
        // AD #54 - Note: Cross Aggregate Orchestration is done in the Handler.
        // We still raise an event here to signify successful verification of THIS aggregate.
        AddDomainEvent(new OtpVerifiedDomainEvent(Id, CitizenIdentityId, now));
    }

    public void Revoke(string reason)
    {
        if (Status == OtpStatus.Pending)
        {
            Status = OtpStatus.Cancelled;
            // Optionally store revocation reason if the domain model dictates it,
            // but AD #55 just says older OTPs must be revoked.
        }
    }
}
