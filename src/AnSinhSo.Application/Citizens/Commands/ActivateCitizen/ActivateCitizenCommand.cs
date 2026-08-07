using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.ActivateCitizen;

/// <summary>
/// Lệnh kích hoạt công dân.
/// </summary>
/// <param name="CitizenId">Định danh công dân.</param>
public sealed record ActivateCitizenCommand(Guid CitizenId) : IRequest<Result>;
