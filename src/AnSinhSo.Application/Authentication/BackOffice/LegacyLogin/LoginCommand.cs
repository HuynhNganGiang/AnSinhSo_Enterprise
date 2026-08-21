using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.BackOffice.Login;

public sealed record LoginCommand(
    string PhoneNumber,
    string OtpCode,
    string IpAddress,
    string UserAgent,
    string DeviceName
) : IRequest<Result<AuthenticationResult>>;
