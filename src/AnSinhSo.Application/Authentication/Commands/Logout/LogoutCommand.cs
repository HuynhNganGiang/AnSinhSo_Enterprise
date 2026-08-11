using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Commands.Logout;

public sealed record LogoutCommand(Guid UserSessionId) : IRequest<Result>;
