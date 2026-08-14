using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.AddHouseholdMember;

/// <summary>
/// Command dùng để thêm một thành viên mới vào hộ gia đình.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình cần thêm thành viên.</param>
/// <param name="CitizenId">Định danh của công dân được thêm vào hộ.</param>
/// <param name="RelationshipTypeId">Định danh của loại quan hệ với chủ hộ.</param>
public sealed record AddHouseholdMemberCommand(
    Guid HouseholdId,
    Guid CitizenId,
    Guid RelationshipTypeId) : IRequest<Result>;
