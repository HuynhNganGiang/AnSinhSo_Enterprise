using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface ITokenIssuingService
{
    Task<Result<AuthenticationResult>> IssueTokensAsync(
        CitizenIdentity identity,
        string deviceName,
        string browser,
        string os,
        string platform,
        string ipAddress,
        string fingerprint,
        bool rememberMe,
        CancellationToken cancellationToken = default);
}
