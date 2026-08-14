using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.UpdateHouseholdAddress;

/// <summary>
/// Command dùng để cập nhật địa chỉ hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh hộ gia đình.</param>
/// <param name="Address">Địa chỉ mới.</param>
public sealed record UpdateHouseholdAddressCommand(
    Guid HouseholdId,
    string Address) : IRequest<Result>;
