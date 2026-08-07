using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Queries.GetPolicyList;

/// <summary>
/// Truy vấn danh sách chính sách.
/// </summary>
/// <param name="PageNumber">Số trang (mặc định 1).</param>
/// <param name="PageSize">Số lượng phần tử trên mỗi trang (mặc định 10).</param>
/// <param name="Keyword">Từ khóa tìm kiếm (theo tên hoặc mô tả).</param>
/// <param name="Status">Trạng thái của chính sách (tùy chọn).</param>
public sealed record GetPolicyListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Keyword = null,
    int? Status = null) : IRequest<Result>;
