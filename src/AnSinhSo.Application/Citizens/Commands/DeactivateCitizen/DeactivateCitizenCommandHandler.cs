using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.Citizens.Commands.DeactivateCitizen;

/// <summary>
/// Handler xử lý lệnh hủy kích hoạt công dân.
/// </summary>
public sealed class DeactivateCitizenCommandHandler : IRequestHandler<DeactivateCitizenCommand, Result>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo DeactivateCitizenCommandHandler.
    /// </summary>
    public DeactivateCitizenCommandHandler(ICitizenRepository citizenRepository, IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh hủy kích hoạt công dân.
    /// </summary>
    public async Task<Result> Handle(DeactivateCitizenCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Citizen), request.CitizenId));
        }

        var result = citizen.Deactivate();
        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
