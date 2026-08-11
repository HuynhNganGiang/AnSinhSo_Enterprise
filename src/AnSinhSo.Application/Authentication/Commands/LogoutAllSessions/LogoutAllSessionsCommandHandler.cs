using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Commands.LogoutAllSessions;

public sealed class LogoutAllSessionsCommandHandler : IRequestHandler<LogoutAllSessionsCommand, Result>
{
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutAllSessionsCommandHandler(IUserSessionRepository userSessionRepository, IUnitOfWork unitOfWork)
    {
        _userSessionRepository = userSessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutAllSessionsCommand request, CancellationToken cancellationToken)
    {
        var activeSessions = await _userSessionRepository.GetActiveSessionsByCitizenAsync(request.CitizenIdentityId, cancellationToken);

        if (activeSessions.Count == 0)
        {
            return Result.Success(); // Idempotent logic
        }

        foreach (var session in activeSessions)
        {
            session.Revoke("User requested logout from all devices");
            _userSessionRepository.Update(session);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
