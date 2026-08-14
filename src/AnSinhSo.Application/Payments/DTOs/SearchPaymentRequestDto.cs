using System;

namespace AnSinhSo.Application.Payments.DTOs;

public record SearchPaymentRequestDto(
    string? Keyword,
    Guid? CitizenId,
    Guid? HouseholdId,
    Guid? WelfareCaseId,
    int? StatusId,
    int? MethodId,
    DateTime? FromDate,
    DateTime? ToDate,
    int Page = 1,
    int PageSize = 10,
    string? Sort = null
);
