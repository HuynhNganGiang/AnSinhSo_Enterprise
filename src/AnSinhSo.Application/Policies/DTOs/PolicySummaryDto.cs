using System;

namespace AnSinhSo.Application.Policies.DTOs;

public record PolicySummaryDto(
    Guid Id,
    string Name,
    decimal Amount,
    int Status);
