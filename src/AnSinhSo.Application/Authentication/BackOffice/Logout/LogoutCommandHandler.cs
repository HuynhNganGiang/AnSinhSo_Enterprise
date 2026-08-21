using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.BackOffice.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly ISecurityRepository _securityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(ISecurityRepository securityRepository, IUnitOfWork unitOfWork)
    {
        _securityRepository = securityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var session = await _securityRepository.GetDeviceSessionByIdAsync(new DeviceSessionId(request.UserSessionId), cancellationToken);

        if (session is null)
        {
            return Result.Failure(SessionErrors.NotFound);
        }

        if (!session.IsActive())
        {
            return Result.Success(); // Idempotent logic: already revoked is treated as success for logout
        }

        session.Revoke("User requested logout");
        _securityRepository.UpdateDeviceSession(session);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
