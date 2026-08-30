using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Security;

public sealed class SessionCleanupService : ISessionCleanupService
{
    private readonly ISecurityRepository _securityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SessionCleanupService> _logger;

    public SessionCleanupService(
        ISecurityRepository securityRepository,
        IUnitOfWork unitOfWork,
        ILogger<SessionCleanupService> logger)
    {
        _securityRepository = securityRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task CleanupExpiredSessionsAsync(
        CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-90);

        var sessionsToArchive =
            await _securityRepository.GetSessionsForArchivalAsync(
                cutoffDate,
                cancellationToken);

        if (sessionsToArchive.Count == 0)
        {
            return;
        }

        foreach (var session in sessionsToArchive)
        {
            session.Archive();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Archived {Count} expired/revoked device sessions.",
            sessionsToArchive.Count);
    }
}
