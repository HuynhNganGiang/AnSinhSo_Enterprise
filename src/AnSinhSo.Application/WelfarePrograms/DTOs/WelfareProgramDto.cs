using System;

namespace AnSinhSo.Application.WelfarePrograms.DTOs;

public record WelfareProgramDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
