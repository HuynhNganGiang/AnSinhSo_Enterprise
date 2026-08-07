using System;
using System.Collections.Generic;

namespace AnSinhSo.Application.Households.DTOs;

public record HouseholdMemberDto(
    Guid Id,
    Guid CitizenId,
    string CitizenName,
    bool IsHead,
    DateTime JoinedDate);

public record HouseholdDto(
    Guid Id,
    string HouseholdCode,
    Guid? HeadCitizenId,
    string HeadCitizenName,
    string Address,
    int Status,
    IReadOnlyList<HouseholdMemberDto> Members);
