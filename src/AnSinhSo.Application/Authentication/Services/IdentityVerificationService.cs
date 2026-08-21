using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Application.Authentication.Services;

public sealed class IdentityVerificationService : IIdentityVerificationService
{
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly ILogger<IdentityVerificationService> _logger;

    public IdentityVerificationService(
        ICitizenIdentityRepository citizenIdentityRepository,
        ILogger<IdentityVerificationService> logger)
    {
        _citizenIdentityRepository = citizenIdentityRepository;
        _logger = logger;
    }

    public async Task<Result<CitizenIdentity>> MarkAsVerifiedAsync(CitizenIdentityId identityId, CancellationToken cancellationToken = default)
    {
        var identity = await _citizenIdentityRepository.GetByIdAsync(identityId, cancellationToken);
        if (identity == null)
        {
            return Result.Failure<CitizenIdentity>(Error.NotFound("Identity.NotFound", "The specified identity was not found."));
        }

        try
        {
            if (identity.PrimaryPhone != null)
            {
                identity.VerifyPhoneNumber(identity.PrimaryPhone, DateTime.UtcNow);
            }
            else
            {
                // Edge case: if they have no primary phone, we can't verify it this way.
                // Normally an identity created for phone auth has a PrimaryPhone.
                return Result.Failure<CitizenIdentity>(Error.Validation("Identity.NoPhone", "Identity does not have a primary phone to verify."));
            }

            _citizenIdentityRepository.Update(identity);

            return Result.Success(identity);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<CitizenIdentity>(Error.Validation("Identity.VerificationFailed", ex.Message));
        }
    }
}
