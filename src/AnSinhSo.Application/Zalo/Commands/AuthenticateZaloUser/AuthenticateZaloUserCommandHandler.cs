using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.Zalo;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;


using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.Interfaces.Repositories;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Application.Zalo.Commands.AuthenticateZaloUser;

public sealed class AuthenticateZaloUserCommandHandler : IRequestHandler<AuthenticateZaloUserCommand, Result<AuthenticationResult>>
{
    private readonly IZaloOAService _zaloOAService;
    private readonly IZaloUserRepository _zaloUserRepository;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISecurityRepository _securityRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthenticationOptions _options;

    public AuthenticateZaloUserCommandHandler(
        IZaloOAService zaloOAService,
        IZaloUserRepository zaloUserRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IUserRepository userRepository,
        ISecurityRepository securityRepository,
        IHashProvider hashProvider,
        IJwtProvider jwtProvider,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<AuthenticationOptions> options)
    {
        _zaloOAService = zaloOAService;
        _zaloUserRepository = zaloUserRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _userRepository = userRepository;
        _securityRepository = securityRepository;
        _hashProvider = hashProvider;
        _jwtProvider = jwtProvider;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<AuthenticationResult>> Handle(AuthenticateZaloUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Exchange auth code for access token
        var accessToken = await _zaloOAService.GetAccessTokenAsync(request.AuthorizationCode, cancellationToken);
        if (string.IsNullOrEmpty(accessToken))
        {
            return Result.Failure<AuthenticationResult>(Error.Validation("Zalo.AuthFailed", "Không thể xác thực Zalo."));
        }

        // 2. Get User Profile from Zalo
        // Assume GetUserProfileAsync uses the accessToken here, or we use a hardcoded ZaloId for simplicity since it's just mock service.
        var zaloProfile = await _zaloOAService.GetUserProfileAsync(accessToken, cancellationToken);
        if (zaloProfile == null || string.IsNullOrEmpty(zaloProfile.Id))
        {
            return Result.Failure<AuthenticationResult>(Error.Validation("Zalo.ProfileFailed", "Không thể lấy thông tin Zalo."));
        }

        // 3. Sync Zalo User in Database
        var zaloUser = await _zaloUserRepository.GetByZaloIdAsync(zaloProfile.Id, cancellationToken);
        if (zaloUser == null)
        {
            zaloUser = ZaloUser.Create(zaloProfile.Id, zaloProfile.Name, zaloProfile.Avatar);
            _zaloUserRepository.Add(zaloUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // Sync profile
            zaloUser.UpdateProfile(zaloProfile.Name, zaloProfile.Avatar);
            _zaloUserRepository.Update(zaloUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // 4. Check if linked
        if (!zaloUser.CitizenIdentityId.HasValue)
        {
            return Result.Failure<AuthenticationResult>(Error.Validation("Zalo.NotLinked", $"Tài khoản Zalo {zaloProfile.Id} chưa được liên kết."));
        }

        // 5. Get linked CitizenIdentity
        var identity = await _citizenIdentityRepository.GetByIdAsync(new CitizenIdentityId(zaloUser.CitizenIdentityId.Value), cancellationToken);
        if (identity == null || identity.Status != AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Verified)
        {
            return Result.Failure<AuthenticationResult>(Error.Validation("Zalo.IdentityInvalid", "Tài khoản AnSinhSo không hợp lệ hoặc đã bị khóa."));
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

        // 6. Device Limits & Save Transaction
        int maxRetries = 2;
        string appAccessToken = string.Empty;
        string finalRawRefreshToken = string.Empty;

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
                        s.Revoke("Device limit exceeded (Zalo Login)");
                        _securityRepository.UpdateDeviceSession(s);
                    }
                }

                var deviceSession = AnSinhSo.Domain.Aggregates.SecurityAggregate.DeviceSession.Create(
                    user.Id.Value,
                    request.DeviceName,
                    "Unknown", // Browser
                    "Unknown", // OS
                    "ZaloClient", // Platform
                    request.IpAddress,
                    "Unknown", // Fingerprint
                    isTrusted: false,
                    securityStamp: user.SecurityStamp
                );

                finalRawRefreshToken = _tokenGenerator.GenerateRefreshToken();
                var hashedRefreshToken = _hashProvider.Hash(finalRawRefreshToken);
                var familyId = Guid.NewGuid();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays);

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

                appAccessToken = _jwtProvider.GenerateAccessToken(identity, deviceSession.Id);
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

        // 7. Return Result
        var jwtExpiryMinutes = _options.AccessTokenLifetimeMinutes;

        var result = new AuthenticationResult(
            appAccessToken,
            finalRawRefreshToken,
            jwtExpiryMinutes * 60,
            identity.Id.Value);

        return Result.Success(result);
    }
}
