using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Application.Authentication.Citizen;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Application.Authentication.Services;

public sealed class OtpVerificationService : IOtpVerificationService
{
    private readonly IOtpVerificationRepository _otpRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IHashProvider _hashProvider;
    private readonly IOtpDeliveryStrategy _deliveryStrategy;
    private readonly ILogger<OtpVerificationService> _logger;
    private readonly OtpOptions _otpOptions;

    public OtpVerificationService(
        IOtpVerificationRepository otpRepository,
        IOtpGenerator otpGenerator,
        IHashProvider hashProvider,
        IOtpDeliveryStrategy deliveryStrategy,
        ILogger<OtpVerificationService> logger,
        IOptions<OtpOptions> otpOptions)
    {
        _otpRepository = otpRepository;
        _otpGenerator = otpGenerator;
        _hashProvider = hashProvider;
        _deliveryStrategy = deliveryStrategy;
        _logger = logger;
        _otpOptions = otpOptions.Value;
    }

    public async Task<Result> GenerateAndSendOtpAsync(CitizenIdentityId identityId, string phoneNumber, Guid requestId, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Generate Raw OTP
            var rawOtp = _otpGenerator.Generate(_otpOptions.CodeLength);
            var codeHash = _hashProvider.Hash(rawOtp);
            var expiresAt = DateTime.UtcNow.AddMinutes(_otpOptions.ExpiryMinutes);

            PhoneNumber targetPhone;
            try
            {
                targetPhone = PhoneNumber.Create(phoneNumber);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure(Error.Validation("PhoneNumber.Invalid", ex.Message));
            }

            // 2. Create Aggregate
            var otpVerification = OtpVerification.Create(
                identityId,
                requestId,
                codeHash,
                targetPhone,
                expiresAt);

            _otpRepository.Add(otpVerification);

            // 3. Send OTP using Strategy Pattern
            // Since there is no explicit provider type requested from the UI in this sprint, we pass null to allow fallback.
            var result = await _deliveryStrategy.DeliverOtpAsync(string.Empty, phoneNumber, rawOtp, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result.Failure(Error.Failure("Otp.SendFailed", result.ErrorMessage ?? "Could not send OTP via any configured providers."));
            }

            return Result.Success(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating and sending OTP for identity {IdentityId}", identityId.Value);
            return Result.Failure(Error.Failure("Otp.Error", "An error occurred while generating the OTP."));
        }
    }

    public async Task<Result> VerifyOtpAsync(Guid requestId, string code, CancellationToken cancellationToken = default)
    {
        try
        {
            var otpVerification = await _otpRepository.GetByRequestIdAsync(requestId, cancellationToken);
            if (otpVerification == null)
            {
                return Result.Failure(Error.NotFound("Otp.NotFound", "OTP request not found or invalid."));
            }

            var hashedCode = _hashProvider.Hash(code);

            // Domain logic handles the state transition, expiration and attempts checking
            otpVerification.Verify(hashedCode, _otpOptions.MaxFailedAttempts, DateTime.UtcNow);

            _otpRepository.Update(otpVerification);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(Error.Validation("Otp.VerificationFailed", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying OTP for request {RequestId}", requestId);
            return Result.Failure(Error.Failure("Otp.Error", "An unexpected error occurred during verification."));
        }
    }
}
