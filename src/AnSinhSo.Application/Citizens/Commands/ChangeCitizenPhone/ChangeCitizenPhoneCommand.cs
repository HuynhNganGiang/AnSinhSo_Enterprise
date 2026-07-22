using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.ChangeCitizenPhone;

/// <summary>
/// Lệnh thay đổi số điện thoại của công dân.
/// </summary>
/// <param name="CitizenId">Định danh công dân.</param>
/// <param name="PhoneNumber">Số điện thoại mới.</param>
public sealed record ChangeCitizenPhoneCommand(
    Guid CitizenId,
    string PhoneNumber) : IRequest<Result>;
