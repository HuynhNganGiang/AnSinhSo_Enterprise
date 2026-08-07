using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.RemoveHouseholdMember;

/// <summary>
/// Command dùng để xóa một thành viên khỏi hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình.</param>
/// <param name="CitizenId">Định danh của công dân cần xóa khỏi hộ.</param>
public sealed record RemoveHouseholdMemberCommand(
    Guid HouseholdId,
    Guid CitizenId) : IRequest<Result>;
