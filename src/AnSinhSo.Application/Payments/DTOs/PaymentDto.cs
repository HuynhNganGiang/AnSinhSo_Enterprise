using System;

namespace AnSinhSo.Application.Payments.DTOs;

public record PaymentDto(
    Guid Id,
    string PaymentNumber,
    Guid CitizenId,
    Guid? HouseholdId,
    Guid WelfareCaseId,
    decimal Amount,
    DateTime ScheduledDate,
    DateTime? ActualPaymentDate,
    string Method,
    string Status,
    string? Notes
);
