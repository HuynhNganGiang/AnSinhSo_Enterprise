using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Audit;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Infrastructure.Audit;

public sealed class LoggerAuditService : IAuditService
{
    private readonly ILogger<LoggerAuditService> _logger;

    public LoggerAuditService(ILogger<LoggerAuditService> logger)
    {
        _logger = logger;
    }

    public Task LogEventAsync(string eventName, string entityId, string details, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Audit Event: {EventName} | EntityId: {EntityId} | Details: {Details}", eventName, entityId, details);
        return Task.CompletedTask;
    }
}
