using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Zalo.Commands.LinkZaloUser;

public sealed record LinkZaloUserCommand(
    string ZaloUserId,
    Guid CitizenIdentityId
) : IRequest<Result>;
