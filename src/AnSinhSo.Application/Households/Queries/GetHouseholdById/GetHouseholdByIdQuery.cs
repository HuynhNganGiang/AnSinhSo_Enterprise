using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Application.Households.DTOs;

namespace AnSinhSo.Application.Households.Queries.GetHouseholdById;

/// <summary>
/// Truy vấn thông tin chi tiết hộ gia đình theo Id.
/// </summary>
/// <param name="HouseholdId">Định danh của hộ gia đình cần lấy thông tin.</param>
public sealed record GetHouseholdByIdQuery(Guid HouseholdId) : IRequest<Result<HouseholdDto>>;
