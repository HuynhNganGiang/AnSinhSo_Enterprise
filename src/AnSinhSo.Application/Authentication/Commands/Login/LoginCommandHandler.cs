using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Application.Authentication.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResult>>
{
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IOtpVerificationRepository _otpVerificationRepository;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthenticationOptions _options;

    public LoginCommandHandler(
        ICitizenIdentityRepository citizenIdentityRepository,
        IOtpVerificationRepository otpVerificationRepository,
        IUserSessionRepository userSessionRepository,
        IHashProvider hashProvider,
        IJwtProvider jwtProvider,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<AuthenticationOptions> options)
    {
        _citizenIdentityRepository = citizenIdentityRepository;
        _otpVerificationRepository = otpVerificationRepository;
        _userSessionRepository = userSessionRepository;
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

        if (identity.Status != IdentityStatus.Active)
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

        // 3. Manage Device Limits (AD #87)
        var activeSessions = await _userSessionRepository.GetActiveSessionsByCitizenAsync(identity.Id.Value, cancellationToken);
        if (activeSessions.Count >= 5)
        {
            // Revoke the oldest session
            var oldestSession = activeSessions.OrderBy(s => s.ExpiresAt).First(); // ExpiresAt relates to when it was created since lifetime is fixed
            oldestSession.Revoke("Device limit exceeded");
            _userSessionRepository.Update(oldestSession);
        }

        // 4. Generate Tokens
        var rawRefreshToken = _tokenGenerator.GenerateRefreshToken();
        var hashedRefreshToken = _hashProvider.Hash(rawRefreshToken);
        
        var deviceInfo = DeviceInfo.Create(request.IpAddress, request.UserAgent, request.DeviceName);
        var familyId = Guid.NewGuid();
        
        var refreshTokenExpiryDays = _options.RefreshTokenLifetimeDays;
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        var newSession = UserSession.Create(
            identity.Id.Value,
            familyId,
            deviceInfo,
            hashedRefreshToken,
            refreshTokenExpiry);

        _userSessionRepository.Add(newSession);

        // 5. Generate JWT
        var jwtExpiryMinutes = _options.AccessTokenLifetimeMinutes;
        if (jwtExpiryMinutes > 15)
        {
            // Enforce AD #86 Fail Fast (Alternatively this could be at Startup, but enforcing here adds extra safety)
            throw new InvalidOperationException("Access Token Lifetime exceeds maximum allowed (15 minutes).");
        }

        var accessToken = _jwtProvider.GenerateAccessToken(identity, newSession.Id);

        // 6. Save Transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new AuthenticationResult(
            accessToken,
            rawRefreshToken,
            jwtExpiryMinutes * 60);

        return Result.Success(result);
    }
}
