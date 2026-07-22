using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.WelfareGroups.Queries.GetWelfareGroupList;

/// <summary>
/// Handler xử lý truy vấn danh sách nhóm phúc lợi.
/// </summary>
public sealed class GetWelfareGroupListQueryHandler : IRequestHandler<GetWelfareGroupListQuery, Result>
{
    private readonly IWelfareGroupRepository _welfareGroupRepository;

    /// <summary>
    /// Khởi tạo GetWelfareGroupListQueryHandler.
    /// </summary>
    public GetWelfareGroupListQueryHandler(IWelfareGroupRepository welfareGroupRepository)
    {
        _welfareGroupRepository = welfareGroupRepository;
    }

    /// <summary>
    /// Xử lý truy vấn danh sách nhóm phúc lợi.
    /// </summary>
    public async Task<Result> Handle(GetWelfareGroupListQuery request, CancellationToken cancellationToken)
    {
        // TODO Step 17: Repository call and Mapping to DTO
        return await Task.FromResult(Result.Success());
    }
}
