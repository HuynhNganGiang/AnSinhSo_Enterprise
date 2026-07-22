using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.DeactivateCitizen;

/// <summary>
/// Lệnh hủy kích hoạt công dân.
/// </summary>
/// <param name="CitizenId">Định danh công dân.</param>
public sealed record DeactivateCitizenCommand(Guid CitizenId) : IRequest<Result>;
