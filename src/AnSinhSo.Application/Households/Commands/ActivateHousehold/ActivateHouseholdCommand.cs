using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.ActivateHousehold;

/// <summary>
/// Command dùng để kích hoạt lại hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình cần kích hoạt.</param>
public sealed record ActivateHouseholdCommand(
    Guid HouseholdId) : IRequest<Result>;
