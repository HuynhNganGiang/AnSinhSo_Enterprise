using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken,
    string IpAddress,
    string UserAgent,
    string DeviceName
) : IRequest<Result<AuthenticationResult>>;
