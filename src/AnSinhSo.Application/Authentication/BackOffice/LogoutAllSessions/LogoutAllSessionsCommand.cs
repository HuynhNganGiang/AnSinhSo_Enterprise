using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.BackOffice.LogoutAllSessions;

public sealed record LogoutAllSessionsCommand(Guid UserId) : IRequest<Result>;
