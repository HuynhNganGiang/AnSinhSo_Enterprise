using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.BackOffice.Logout;

public sealed record LogoutCommand(Guid SessionId) : IRequest<Result>;
