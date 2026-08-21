using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.BackOffice.LogoutAllSessions;

public sealed class LogoutAllSessionsCommandHandler : IRequestHandler<LogoutAllSessionsCommand, Result>
{
    private readonly ISecurityRepository _securityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutAllSessionsCommandHandler(ISecurityRepository securityRepository, IUnitOfWork unitOfWork)
    {
        _securityRepository = securityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutAllSessionsCommand request, CancellationToken cancellationToken)
    {
        var activeSessions = await _securityRepository.GetActiveDeviceSessionsByUserIdAsync(request.UserId, cancellationToken);

        if (activeSessions.Count == 0)
        {
            return Result.Success(); // Idempotent logic
        }

        foreach (var session in activeSessions)
        {
            session.Revoke("User requested logout from all devices");
            _securityRepository.UpdateDeviceSession(session);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
