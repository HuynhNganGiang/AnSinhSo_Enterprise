using System;

namespace AnSinhSo.Application.WelfareCases.DTOs;

public record WelfareCaseDto(
    Guid Id,
    Guid CitizenId,
    Guid? HouseholdId,
    Guid ProgramId,
    string Status,
    string? Notes,
    decimal? BenefitAmount,
    DateTime? EffectiveFrom,
    DateTime? EffectiveTo,
    CitizenSnapshotDto CitizenSnapshot);

public record CitizenSnapshotDto(
    string CitizenNumber,
    string FullName,
    DateTime DateOfBirth,
    string Gender,
    string? HouseholdCode,
    string Address,
    string? Phone,
    DateTime CreatedAtSnapshot);
