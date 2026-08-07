using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.WelfareGroups.Commands.ActivateWelfareGroup;

/// <summary>
/// Handler xử lý lệnh kích hoạt nhóm phúc lợi.
/// </summary>
public sealed class ActivateWelfareGroupCommandHandler : IRequestHandler<ActivateWelfareGroupCommand, Result>
{
    private readonly IWelfareGroupRepository _welfareGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ActivateWelfareGroupCommandHandler.
    /// </summary>
    public ActivateWelfareGroupCommandHandler(
        IWelfareGroupRepository welfareGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _welfareGroupRepository = welfareGroupRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh kích hoạt nhóm phúc lợi.
    /// </summary>
    public async Task<Result> Handle(ActivateWelfareGroupCommand request, CancellationToken cancellationToken)
    {
        var welfareGroupId = new WelfareGroupId(request.WelfareGroupId);
        var welfareGroup = await _welfareGroupRepository.GetByIdAsync(welfareGroupId, cancellationToken);

        if (welfareGroup is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(WelfareGroup), request.WelfareGroupId));
        }

        var result = welfareGroup.Activate();

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
