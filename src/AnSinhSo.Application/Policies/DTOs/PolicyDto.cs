using System;

namespace AnSinhSo.Application.Policies.DTOs;

public record PolicyDto(
    Guid Id,
    string Name,
    string Description,
    decimal Amount,
    int Status,
    DateTime CreatedDate);
