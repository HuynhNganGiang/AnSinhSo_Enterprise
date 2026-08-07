using System;

namespace AnSinhSo.Application.Citizens.DTOs;

public record CitizenSummaryDto(
    Guid Id,
    string FullName,
    string CitizenNumber,
    int Status);
