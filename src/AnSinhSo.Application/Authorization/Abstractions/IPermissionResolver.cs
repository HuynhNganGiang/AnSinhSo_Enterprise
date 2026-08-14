using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;

namespace AnSinhSo.Application.Authorization.Abstractions;

public interface IPermissionResolver
{
    Task<IReadOnlyCollection<string>> GetPermissionsAsync(CitizenIdentityId citizenIdentityId, CancellationToken cancellationToken = default);
}
