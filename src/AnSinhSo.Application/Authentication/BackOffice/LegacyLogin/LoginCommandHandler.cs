using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;


using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Application.Authentication.BackOffice.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResult>>
{
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IOtpVerificationRepository _otpVerificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISecurityRepository _securityRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthenticationOptions _options;

    public LoginCommandHandler(
        ICitizenIdentityRepository citizenIdentityRepository,
        IOtpVerificationRepository otpVerificationRepository,
        IUserRepository userRepository,
        ISecurityRepository securityRepository,
        IHashProvider hashProvider,
        IJwtProvider jwtProvider,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<AuthenticationOptions> options)
    {
        _citizenIdentityRepository = citizenIdentityRepository;
        _otpVerificationRepository = otpVerificationRepository;
        _userRepository = userRepository;
        _securityRepository = securityRepository;
        _hashProvider = hashProvider;
        _jwtProvider = jwtProvider;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var targetPhone = PhoneNumber.Create(request.PhoneNumber);

        // 1. Fetch Citizen Identity by Phone
        var identity = await _citizenIdentityRepository.GetByPhoneNumberAsync(targetPhone, cancellationToken);
        if (identity is null)
        {
            return Result.Failure<AuthenticationResult>(IdentityErrors.IdentityNotFound);
        }

        if (identity.Status != IdentityStatus.Verified)
        {
            return Result.Failure<AuthenticationResult>(IdentityErrors.IdentityNotActive);
        }

        // 2. Fetch Pending OTP and verify
        var pendingOtp = await _otpVerificationRepository.GetPendingByCitizenIdentityIdAsync(identity.Id, cancellationToken);
        if (pendingOtp is null)
        {
            return Result.Failure<AuthenticationResult>(OtpErrors.NotFound);
        }

        var providedHash = _hashProvider.Hash(request.OtpCode);

        try
        {
            // Verify OTP
            pendingOtp.Verify(providedHash, maxAttempts: 5, DateTime.UtcNow);

            // Mark as used is handled inside Verify() domain method, but we still need to update
            _otpVerificationRepository.Update(pendingOtp);
        }
        catch (InvalidOperationException ex)
        {
            _otpVerificationRepository.Update(pendingOtp);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (ex.Message.Contains("expired", StringComparison.OrdinalIgnoreCase))
                return Result.Failure<AuthenticationResult>(OtpErrors.Expired);

            if (ex.Message.Contains("revoked", StringComparison.OrdinalIgnoreCase))
                return Result.Failure<AuthenticationResult>(OtpErrors.ExceedMaxAttempts);

            if (ex.Message.Contains("already been used", StringComparison.OrdinalIgnoreCase))
                return Result.Failure<AuthenticationResult>(OtpErrors.AlreadyUsed);

            if (ex.Message.Contains("Invalid OTP code", StringComparison.OrdinalIgnoreCase))
                return Result.Failure<AuthenticationResult>(OtpErrors.Invalid);

            return Result.Failure<AuthenticationResult>(Error.Validation("Otp.VerificationFailed", ex.Message));
        }

        if (identity.PrimaryPhone == null)
        {
            return Result.Failure<AuthenticationResult>(Error.Validation("Identity.NoPhone", "Identity does not have a primary phone."));
        }

        var user = await _userRepository.GetByUsernameAsync(identity.PrimaryPhone.Value, cancellationToken);
        if (user == null)
        {
            return Result.Failure<AuthenticationResult>(Error.NotFound("User.NotFound", "The verified identity does not have an associated active user account."));
        }

        // 5. Manage Device Limits & Save Transaction
        int maxRetries = 2;
        string accessToken = string.Empty;
        string finalRawRefreshToken = string.Empty;
        int jwtExpiryMinutes = _options.AccessTokenLifetimeMinutes;

        if (jwtExpiryMinutes > 15)
        {
            throw new InvalidOperationException("Access Token Lifetime exceeds maximum allowed (15 minutes).");
        }

        for (int retry = 0; retry <= maxRetries; retry++)
        {
            try
            {
                var activeSessions = await _securityRepository.GetActiveDeviceSessionsByUserIdAsync(user.Id.Value, cancellationToken);
                if (activeSessions.Count >= 5)
                {
                    var sessionsToRevoke = activeSessions.OrderBy(s => s.LastSeenAt).Take(activeSessions.Count - 4);
                    foreach (var s in sessionsToRevoke)
                    {
                        s.Revoke("Device limit exceeded (Legacy Login)");
                        _securityRepository.UpdateDeviceSession(s);
                    }
                }

                // Generate new objects per retry
                finalRawRefreshToken = _tokenGenerator.GenerateRefreshToken();
                var hashedRefreshToken = _hashProvider.Hash(finalRawRefreshToken);
                var familyId = Guid.NewGuid();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays);

                var deviceSession = AnSinhSo.Domain.Aggregates.SecurityAggregate.DeviceSession.Create(
                    user.Id.Value,
                    request.DeviceName,
                    "Unknown", // Browser
                    "Unknown", // OS
                    "LegacyClient", // Platform
                    request.IpAddress,
                    "Unknown", // Fingerprint
                    isTrusted: false,
                    securityStamp: user.SecurityStamp
                );

                var securityRefreshToken = AnSinhSo.Domain.Aggregates.SecurityAggregate.RefreshToken.Create(
                    user.Id.Value,
                    identity.Id.Value,
                    hashedRefreshToken,
                    familyId,
                    refreshTokenExpiry,
                    deviceSession.Id);

                deviceSession.LinkRefreshToken(securityRefreshToken.Id.Value);

                _securityRepository.AddDeviceSession(deviceSession);
                _securityRepository.AddRefreshToken(securityRefreshToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                accessToken = _jwtProvider.GenerateAccessToken(identity, deviceSession.Id);
                break;
            }
            catch (AnSinhSo.Domain.Exceptions.ConcurrencyException)
            {
                if (retry == maxRetries)
                {
                    return Result.Failure<AuthenticationResult>(Error.Conflict("Auth.Concurrency", "Xung đột dữ liệu khi đăng nhập. Vui lòng thử lại."));
                }

                _unitOfWork.ClearChangeTracker();
            }
        }

        var result = new AuthenticationResult(
            accessToken,
            finalRawRefreshToken,
            jwtExpiryMinutes * 60,
            identity.Id.Value);

        return Result.Success(result);
    }
}
