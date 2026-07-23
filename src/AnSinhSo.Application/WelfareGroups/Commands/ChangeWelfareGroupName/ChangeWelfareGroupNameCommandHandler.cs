using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.WelfareGroups.Commands.ChangeWelfareGroupName;

/// <summary>
/// Handler xử lý lệnh thay đổi tên nhóm phúc lợi.
/// </summary>
public sealed class ChangeWelfareGroupNameCommandHandler : IRequestHandler<ChangeWelfareGroupNameCommand, Result>
{
    private readonly IWelfareGroupRepository _welfareGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ChangeWelfareGroupNameCommandHandler.
    /// </summary>
    public ChangeWelfareGroupNameCommandHandler(
        IWelfareGroupRepository welfareGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _welfareGroupRepository = welfareGroupRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh thay đổi tên nhóm phúc lợi.
    /// </summary>
    public async Task<Result> Handle(ChangeWelfareGroupNameCommand request, CancellationToken cancellationToken)
    {
        var welfareGroupId = new WelfareGroupId(request.WelfareGroupId);
        var welfareGroup = await _welfareGroupRepository.GetByIdAsync(welfareGroupId, cancellationToken);

        if (welfareGroup is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(WelfareGroup), request.WelfareGroupId));
        }

        var result = welfareGroup.Rename(request.Name, request.Description);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
