using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Commands.LogoutAllSessions;

public sealed record LogoutAllSessionsCommand(Guid CitizenIdentityId) : IRequest<Result>;
