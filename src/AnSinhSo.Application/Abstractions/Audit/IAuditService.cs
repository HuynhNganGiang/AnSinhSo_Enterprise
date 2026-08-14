using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Audit;

public interface IAuditService
{
    Task LogEventAsync(string eventName, string entityId, string details, CancellationToken cancellationToken = default);
}
