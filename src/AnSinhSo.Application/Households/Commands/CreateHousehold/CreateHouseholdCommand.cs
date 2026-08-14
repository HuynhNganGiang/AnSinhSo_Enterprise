using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.CreateHousehold;

/// <summary>
/// Command dùng để khởi tạo một hộ gia đình mới.
/// </summary>
/// <param name="HouseholdCode">Mã hộ gia đình.</param>
/// <param name="HeadCitizenId">Định danh của công dân làm chủ hộ.</param>
/// <param name="HeadRelationshipTypeId">Định danh của loại quan hệ với chủ hộ.</param>
/// <param name="Address">Địa chỉ của hộ gia đình.</param>
public sealed record CreateHouseholdCommand(
    string HouseholdCode,
    Guid HeadCitizenId,
    Guid HeadRelationshipTypeId,
    string Address) : IRequest<Result<Guid>>;
