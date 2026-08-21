using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Abstractions.Authentication;

public interface IOtpVerificationService
{
    Task<Result> GenerateAndSendOtpAsync(CitizenIdentityId identityId, string phoneNumber, Guid requestId, CancellationToken cancellationToken = default);
    Task<Result> VerifyOtpAsync(Guid requestId, string code, CancellationToken cancellationToken = default);
}
