using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Queries.GetWelfareGroupList;

/// <summary>
/// Truy vấn danh sách nhóm phúc lợi.
/// </summary>
/// <param name="PageNumber">Số trang (mặc định 1).</param>
/// <param name="PageSize">Số lượng phần tử trên mỗi trang (mặc định 10).</param>
/// <param name="Keyword">Từ khóa tìm kiếm (theo tên hoặc mô tả).</param>
/// <param name="IsActive">Trạng thái kích hoạt (tùy chọn).</param>
public sealed record GetWelfareGroupListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Keyword = null,
    bool? IsActive = null) : IRequest<Result>;
