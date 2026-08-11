using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IUserSessionRepository userSessionRepository, IUnitOfWork unitOfWork)
    {
        _userSessionRepository = userSessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var session = await _userSessionRepository.GetByIdAsync(new UserSessionId(request.UserSessionId), cancellationToken);

        if (session is null)
        {
            return Result.Failure(SessionErrors.NotFound);
        }

        if (session.IsRevoked)
        {
            return Result.Success(); // Idempotent logic: already revoked is treated as success for logout
        }

        session.Revoke("User requested logout");
        _userSessionRepository.Update(session);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
