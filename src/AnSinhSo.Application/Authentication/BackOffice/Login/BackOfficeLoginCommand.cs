using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.BackOffice.BackOfficeLogin;

public sealed record BackOfficeLoginCommand(
    string Username,
    string Password,
    string IpAddress,
    string UserAgent,
    string DeviceName,
    bool RememberMe,
    string Language,
    string TimeZone
) : IRequest<Result<AuthenticationResult>>;
