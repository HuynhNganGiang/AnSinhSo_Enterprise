using System;
using System.Collections.Generic;

namespace AnSinhSo.Application.Payments.DTOs;

public record PaymentDetailDto(
    Guid Id,
    Guid CitizenId,
    string CitizenName,
    decimal Amount,
    int Status);

public record PaymentDto(
    Guid Id,
    Guid PolicyId,
    string PolicyName,
    string Description,
    int Status,
    IReadOnlyList<PaymentDetailDto> Details);
