using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;


using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Application.Authentication.Services;

public sealed class TokenIssuingService : ITokenIssuingService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityRepository _securityRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHashProvider _hashProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly AuthenticationOptions _options;

    public TokenIssuingService(
        IUserRepository userRepository,
        ISecurityRepository securityRepository,
        IJwtProvider jwtProvider,
        IHashProvider hashProvider,
        ITokenGenerator tokenGenerator,
        IOptions<AuthenticationOptions> options)
    {
        _userRepository = userRepository;
        _securityRepository = securityRepository;
        _jwtProvider = jwtProvider;
        _hashProvider = hashProvider;
        _tokenGenerator = tokenGenerator;
        _options = options.Value;
    }

    public async Task<Result<AuthenticationResult>> IssueTokensAsync(
        CitizenIdentity identity,
        string deviceName,
        string browser,
        string os,
        string platform,
        string ipAddress,
        string fingerprint,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        if (identity.PrimaryPhone == null)
        {
            return Result.Failure<AuthenticationResult>(Error.Validation("Identity.NoPhone", "Identity does not have a primary phone."));
        }

        // Try to find the user by phone number
        var user = await _userRepository.GetByUsernameAsync(identity.PrimaryPhone.Value, cancellationToken);
        if (user == null)
        {
            return Result.Failure<AuthenticationResult>(Error.NotFound("User.NotFound", "The verified identity does not have an associated active user account."));
        }

        // Create a Device Session
        var deviceSession = DeviceSession.Create(
            user.Id.Value,
            deviceName,
            browser,
            os,
            platform,
            ipAddress,
            fingerprint,
            isTrusted: rememberMe,
            securityStamp: user.SecurityStamp
        );

        _securityRepository.AddDeviceSession(deviceSession);

        var refreshTokenStr = _tokenGenerator.GenerateRefreshToken();
        var hashedRefreshToken = _hashProvider.Hash(refreshTokenStr);
        var expirationDays = rememberMe ? 30 : 1;
        var expirationDate = DateTime.UtcNow.AddDays(expirationDays);
        var familyId = Guid.NewGuid();

        // Persist RefreshToken for DeviceSession (SecurityAggregate)
        var securityRefreshToken = RefreshToken.Create(
            user.Id.Value, identity.Id.Value, hashedRefreshToken, familyId, expirationDate, deviceSession.Id);

        deviceSession.LinkRefreshToken(securityRefreshToken.Id.Value);
        _securityRepository.AddRefreshToken(securityRefreshToken);

        var accessToken = _jwtProvider.GenerateAccessTokenForUser(user, deviceSession.Id);

        return Result.Success(new AuthenticationResult(
            AccessToken: accessToken,
            RefreshToken: refreshTokenStr,
            ExpiresInSeconds: _options.AccessTokenLifetimeMinutes * 60,
            UserId: user.Id.Value
        ));
    }
}
