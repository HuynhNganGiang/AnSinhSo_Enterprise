using System;

namespace AnSinhSo.Application.Citizens.DTOs;

public record CitizenDto(
    Guid Id,
    string FullName,
    string CitizenNumber,
    DateTime BirthDate,
    int Gender,
    string PhoneNumber,
    string Address,
    string Email,
    int Status);
