using System;

namespace AnSinhSo.Application.Households.DTOs;

public record HouseholdSummaryDto(
    Guid Id,
    string HouseholdCode,
    string HeadCitizenName,
    int Status,
    int MemberCount);
