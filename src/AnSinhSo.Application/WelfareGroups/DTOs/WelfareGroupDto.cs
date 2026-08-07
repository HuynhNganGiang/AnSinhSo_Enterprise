using System;

namespace AnSinhSo.Application.WelfareGroups.DTOs;

public record WelfareGroupDto(
    Guid Id,
    string Name,
    string Description,
    bool IsActive);
