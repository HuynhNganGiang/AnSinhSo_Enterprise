using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.WelfareGroups.Queries.GetWelfareGroupById;

/// <summary>
/// Handler xử lý truy vấn lấy thông tin chi tiết nhóm phúc lợi theo ID.
/// </summary>
public sealed class GetWelfareGroupByIdQueryHandler : IRequestHandler<GetWelfareGroupByIdQuery, Result>
{
    private readonly IWelfareGroupRepository _welfareGroupRepository;

    /// <summary>
    /// Khởi tạo GetWelfareGroupByIdQueryHandler.
    /// </summary>
    public GetWelfareGroupByIdQueryHandler(IWelfareGroupRepository welfareGroupRepository)
    {
        _welfareGroupRepository = welfareGroupRepository;
    }

    /// <summary>
    /// Xử lý truy vấn thông tin chi tiết nhóm phúc lợi.
    /// </summary>
    public async Task<Result> Handle(GetWelfareGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var welfareGroupId = new WelfareGroupId(request.WelfareGroupId);
        var welfareGroup = await _welfareGroupRepository.GetByIdAsync(welfareGroupId, cancellationToken);

        if (welfareGroup is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(WelfareGroup), request.WelfareGroupId));
        }

        // TODO Step 17: Return DTO

        return Result.Success();
    }
}
