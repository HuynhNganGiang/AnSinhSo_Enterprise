using MediatR;
using AnSinhSo.Contracts.Authentication;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authentication.Login;

public sealed record LoginCommand(string UsernameOrEmail, string Password) : IRequest<Result<TokenResponseDto>>;
