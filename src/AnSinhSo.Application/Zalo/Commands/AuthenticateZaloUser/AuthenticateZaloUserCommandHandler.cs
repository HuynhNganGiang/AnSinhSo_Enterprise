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
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
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
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AuthenticationOptions _options;

    public AuthenticateZaloUserCommandHandler(
        IZaloOAService zaloOAService,
        IZaloUserRepository zaloUserRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IUserSessionRepository userSessionRepository,
        IHashProvider hashProvider,
        IJwtProvider jwtProvider,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork,
        IOptions<AuthenticationOptions> options)
    {
        _zaloOAService = zaloOAService;
        _zaloUserRepository = zaloUserRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _userSessionRepository = userSessionRepository;
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

        // 6. Device Limits
        var activeSessions = await _userSessionRepository.GetActiveSessionsByCitizenAsync(identity.Id.Value, cancellationToken);
        if (activeSessions.Count() >= 5)
        {
            var oldestSession = activeSessions.OrderBy(s => s.ExpiresAt).First();
            oldestSession.Revoke("Device limit exceeded (Zalo)");
            _userSessionRepository.Update(oldestSession);
        }

        // 7. Generate Tokens
        var rawRefreshToken = _tokenGenerator.GenerateRefreshToken();
        var hashedRefreshToken = _hashProvider.Hash(rawRefreshToken);
        var deviceInfo = DeviceInfo.Create(request.IpAddress, request.UserAgent, request.DeviceName);
        var familyId = Guid.NewGuid();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays);

        var newSession = UserSession.Create(
            identity.Id.Value,
            familyId,
            deviceInfo,
            hashedRefreshToken,
            refreshTokenExpiry);

        _userSessionRepository.Add(newSession);

        var jwtExpiryMinutes = _options.AccessTokenLifetimeMinutes;
        var appAccessToken = _jwtProvider.GenerateAccessToken(identity, newSession.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new AuthenticationResult(
            appAccessToken,
            rawRefreshToken,
            jwtExpiryMinutes * 60,
            Guid.Empty);

        return Result.Success(result);
    }
}
