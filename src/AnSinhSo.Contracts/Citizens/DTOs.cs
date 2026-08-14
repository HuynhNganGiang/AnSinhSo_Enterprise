using System;

namespace AnSinhSo.Contracts.Citizens;

public record CitizenDto(
    Guid Id,
    string FullName,
    string CitizenNumber,
    string PhoneNumber,
    string Email,
    string Status
);

public record CitizenDetailDto(
    Guid Id,
    string FullName,
    string CitizenNumber,
    DateTime BirthDate,
    string Gender,
    string PhoneNumber,
    string Address,
    string Email,
    string Status
);
