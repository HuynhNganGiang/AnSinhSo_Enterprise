using System;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;

namespace AnSinhSo.Domain.Aggregates.OtpVerificationAggregate.Events;

public sealed record OtpVerifiedDomainEvent(
    OtpVerificationId OtpVerificationId,
    CitizenIdentityId CitizenIdentityId,
    DateTime VerifiedAt
) : DomainEvent;
