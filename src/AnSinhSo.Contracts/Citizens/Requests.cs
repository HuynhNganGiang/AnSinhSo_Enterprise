using System;
using AnSinhSo.Contracts.Common;

namespace AnSinhSo.Contracts.Citizens;

public record CreateCitizenRequest(
    string FullName,
    string CitizenNumber,
    DateTime BirthDate,
    int Gender,
    string PhoneNumber,
    string Address,
    string Email
);

public record UpdateCitizenRequest(
    string FullName,
    DateTime BirthDate,
    int Gender,
    string PhoneNumber,
    string Address,
    string Email
);

public record SearchCitizenRequest : PagedRequest
{
    // Căn cước công dân có thể truyền riêng nếu cần, nhưng thường dùng Keyword
    public string? IdentityNumber { get; init; }
    public string? Phone { get; init; }
}
