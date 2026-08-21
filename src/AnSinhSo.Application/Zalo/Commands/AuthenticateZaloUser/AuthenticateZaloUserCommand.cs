using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Zalo.Commands.AuthenticateZaloUser;

public sealed record AuthenticateZaloUserCommand(
    string AuthorizationCode,
    string IpAddress,
    string UserAgent,
    string DeviceName
) : IRequest<Result<AuthenticationResult>>;
