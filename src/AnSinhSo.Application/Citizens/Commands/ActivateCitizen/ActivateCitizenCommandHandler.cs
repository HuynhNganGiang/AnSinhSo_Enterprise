using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.Citizens.Commands.ActivateCitizen;

/// <summary>
/// Handler xử lý lệnh kích hoạt công dân.
/// </summary>
public sealed class ActivateCitizenCommandHandler : IRequestHandler<ActivateCitizenCommand, Result>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo ActivateCitizenCommandHandler.
    /// </summary>
    public ActivateCitizenCommandHandler(ICitizenRepository citizenRepository, IUnitOfWork unitOfWork)
    {
        _citizenRepository = citizenRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh kích hoạt công dân.
    /// </summary>
    public async Task<Result> Handle(ActivateCitizenCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Citizen), request.CitizenId));
        }

        var result = citizen.Activate();
        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
