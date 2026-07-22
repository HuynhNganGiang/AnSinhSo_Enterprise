using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.DeactivateHousehold;

/// <summary>
/// Command dùng để hủy kích hoạt (vô hiệu hóa) hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình cần vô hiệu hóa.</param>
public sealed record DeactivateHouseholdCommand(
    Guid HouseholdId) : IRequest<Result>;
