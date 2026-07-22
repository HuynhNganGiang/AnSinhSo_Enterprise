using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.ChangeHouseholdHead;

/// <summary>
/// Command dùng để thay đổi chủ hộ của một hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình cần thay đổi chủ hộ.</param>
/// <param name="NewHeadCitizenId">Định danh của công dân sẽ trở thành chủ hộ mới.</param>
public sealed record ChangeHouseholdHeadCommand(
    Guid HouseholdId,
    Guid NewHeadCitizenId) : IRequest<Result>;
