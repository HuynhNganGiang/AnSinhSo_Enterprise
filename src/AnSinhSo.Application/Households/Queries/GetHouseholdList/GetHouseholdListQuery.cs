using System.Collections.Generic;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Application.Households.DTOs;

namespace AnSinhSo.Application.Households.Queries.GetHouseholdList;

/// <summary>
/// Truy vấn danh sách hộ gia đình có phân trang và lọc.
/// </summary>
/// <param name="PageNumber">Số trang (bắt đầu từ 1).</param>
/// <param name="PageSize">Số lượng bản ghi trên một trang.</param>
/// <param name="Keyword">Từ khóa tìm kiếm.</param>
/// <param name="IsActive">Lọc theo trạng thái hoạt động.</param>
public sealed record GetHouseholdListQuery(
    int PageNumber,
    int PageSize,
    string? Keyword,
    bool? IsActive) : IRequest<Result<IReadOnlyList<HouseholdDto>>>;
