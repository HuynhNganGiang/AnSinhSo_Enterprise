using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Abstractions.Security;

public interface ISessionCleanupService
{
    Task CleanupExpiredSessionsAsync(CancellationToken cancellationToken = default);
}
