using System;

namespace AnSinhSo.Application.Payments.DTOs;

public record PaymentSummaryDto(
    Guid Id,
    Guid PolicyId,
    string PolicyName,
    int Status,
    decimal TotalAmount);
