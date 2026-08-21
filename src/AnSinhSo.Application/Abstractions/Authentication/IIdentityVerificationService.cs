using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IIdentityVerificationService
{
    Task<Result<CitizenIdentity>> MarkAsVerifiedAsync(CitizenIdentityId identityId, CancellationToken cancellationToken = default);
}
