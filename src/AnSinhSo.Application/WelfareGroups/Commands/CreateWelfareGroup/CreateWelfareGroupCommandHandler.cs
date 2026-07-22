using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;

namespace AnSinhSo.Application.WelfareGroups.Commands.CreateWelfareGroup;

/// <summary>
/// Handler xử lý lệnh tạo mới nhóm phúc lợi.
/// </summary>
public sealed class CreateWelfareGroupCommandHandler : IRequestHandler<CreateWelfareGroupCommand, Result<Guid>>
{
    private readonly IWelfareGroupRepository _welfareGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo CreateWelfareGroupCommandHandler.
    /// </summary>
    public CreateWelfareGroupCommandHandler(
        IWelfareGroupRepository welfareGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _welfareGroupRepository = welfareGroupRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh tạo mới nhóm phúc lợi.
    /// </summary>
    public async Task<Result<Guid>> Handle(CreateWelfareGroupCommand request, CancellationToken cancellationToken)
    {
        var id = new WelfareGroupId(Guid.NewGuid());
        var result = WelfareGroup.Create(id, request.Name, request.Description);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        await _welfareGroupRepository.AddAsync(result.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(id.Value);
    }
}
