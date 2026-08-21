using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Repositories;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Zalo.Commands.HandleZaloWebhook;

public sealed class HandleZaloWebhookCommandHandler : IRequestHandler<HandleZaloWebhookCommand, Result>
{
    private readonly IZaloUserRepository _zaloUserRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HandleZaloWebhookCommandHandler(IZaloUserRepository zaloUserRepository, IUnitOfWork unitOfWork)
    {
        _zaloUserRepository = zaloUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(HandleZaloWebhookCommand request, CancellationToken cancellationToken)
    {
        var zaloUser = await _zaloUserRepository.GetByZaloIdAsync(request.ZaloUserId, cancellationToken);
        
        switch (request.EventName)
        {
            case "follow":
                if (zaloUser == null)
                {
                    // Create minimal user
                    zaloUser = ZaloUser.Create(request.ZaloUserId, "Unknown", null);
                    zaloUser.SetFollowingStatus(true);
                    _zaloUserRepository.Add(zaloUser);
                }
                else
                {
                    zaloUser.SetFollowingStatus(true);
                    _zaloUserRepository.Update(zaloUser);
                }
                break;
                
            case "unfollow":
                if (zaloUser != null)
                {
                    zaloUser.SetFollowingStatus(false);
                    _zaloUserRepository.Update(zaloUser);
                }
                break;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
