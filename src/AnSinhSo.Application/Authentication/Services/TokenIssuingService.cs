using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authentication.Services;

public sealed class TokenIssuingService : ITokenIssuingService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityRepository _securityRepository;
    private readonly IJwtProvider _jwtProvider;

    public TokenIssuingService(
        IUserRepository userRepository,
        ISecurityRepository securityRepository,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _securityRepository = securityRepository;
        _jwtProvider = jwtProvider;
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

        // Normally we would also create a Refresh Token and link it
        // var refreshToken = RefreshToken.Create(user.Id.Value, deviceSession.Id.Value, ...);
        // deviceSession.LinkRefreshToken(refreshToken.Id.Value);

        _securityRepository.AddDeviceSession(deviceSession);
        
        var accessToken = _jwtProvider.GenerateAccessTokenForUser(user, deviceSession.Id);
        // Generate a random string as refresh token for now since RefreshToken aggregate details aren't fully exposed
        var refreshTokenStr = Guid.NewGuid().ToString("N");

        return Result.Success(new AuthenticationResult(
            AccessToken: accessToken,
            RefreshToken: refreshTokenStr,
            ExpiresInSeconds: 3600,
            UserId: user.Id.Value
        ));
    }
}
