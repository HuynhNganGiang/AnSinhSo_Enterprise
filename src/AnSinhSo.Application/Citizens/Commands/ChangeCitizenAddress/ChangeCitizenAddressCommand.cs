using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.ChangeCitizenAddress;

/// <summary>
/// Lệnh thay đổi địa chỉ thường trú của công dân.
/// </summary>
/// <param name="CitizenId">Định danh công dân.</param>
/// <param name="Address">Địa chỉ thường trú mới.</param>
public sealed record ChangeCitizenAddressCommand(
    Guid CitizenId,
    string Address) : IRequest<Result>;
