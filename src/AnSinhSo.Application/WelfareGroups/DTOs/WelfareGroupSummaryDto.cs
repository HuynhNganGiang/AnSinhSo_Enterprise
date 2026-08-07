using System;

namespace AnSinhSo.Application.WelfareGroups.DTOs;

public record WelfareGroupSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
