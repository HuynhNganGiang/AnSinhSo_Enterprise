using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenList;

/// <summary>
/// Truy vấn danh sách công dân.
/// </summary>
/// <param name="PageNumber">Số trang (mặc định 1).</param>
/// <param name="PageSize">Số lượng phần tử trên mỗi trang (mặc định 10).</param>
/// <param name="Keyword">Từ khóa tìm kiếm (theo tên hoặc số thẻ).</param>
/// <param name="Status">Trạng thái công dân (tùy chọn).</param>
public sealed record GetCitizenListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Keyword = null,
    int? Status = null) : IRequest<Result>;
