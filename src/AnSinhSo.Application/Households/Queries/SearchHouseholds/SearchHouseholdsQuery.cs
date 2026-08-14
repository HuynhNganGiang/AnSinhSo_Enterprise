using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using System.Collections.Generic;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Application.Households.DTOs;

using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;
using AnSinhSo.Contracts.Common;

namespace AnSinhSo.Application.Households.Queries.SearchHouseholds;

/// <summary>
/// Truy vấn danh sách hộ gia đình có phân trang và lọc.
/// </summary>
/// <param name="Page">Số trang (bắt đầu từ 1).</param>
/// <param name="PageSize">Số lượng bản ghi trên một trang.</param>
/// <param name="Keyword">Từ khóa tìm kiếm.</param>
/// <param name="Status">Lọc theo trạng thái hoạt động.</param>
/// <param name="Sort">Sắp xếp.</param>
public sealed record SearchHouseholdsQuery(
    int Page,
    int PageSize,
    string? Keyword,
    HouseholdStatus? Status,
    string? Sort) : IRequest<Result<PagedResult<HouseholdSummaryDto>>>;
