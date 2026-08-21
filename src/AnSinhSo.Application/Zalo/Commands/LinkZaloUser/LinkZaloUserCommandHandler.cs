using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Repositories;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Zalo.Commands.LinkZaloUser;

public sealed class LinkZaloUserCommandHandler : IRequestHandler<LinkZaloUserCommand, Result>
{
    private readonly IZaloUserRepository _zaloUserRepository;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LinkZaloUserCommandHandler(
        IZaloUserRepository zaloUserRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IUnitOfWork unitOfWork)
    {
        _zaloUserRepository = zaloUserRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LinkZaloUserCommand request, CancellationToken cancellationToken)
    {
        var zaloUser = await _zaloUserRepository.GetByZaloIdAsync(request.ZaloUserId, cancellationToken);
        if (zaloUser == null)
        {
            return Result.Failure(Error.Validation("Zalo.NotFound", "Không tìm thấy người dùng Zalo."));
        }

        var identity = await _citizenIdentityRepository.GetByIdAsync(new CitizenIdentityId(request.CitizenIdentityId), cancellationToken);
        if (identity == null)
        {
            return Result.Failure(IdentityErrors.IdentityNotFound);
        }

        zaloUser.LinkCitizen(identity.CitizenId.Value, identity.Id.Value, identity.PrimaryPhone?.Value ?? "");
        _zaloUserRepository.Update(zaloUser);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
