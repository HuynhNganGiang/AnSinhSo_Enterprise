using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authentication.BackOffice.RefreshToken.Resolvers;

public sealed class SecurityRefreshResolver : IRefreshSessionResolver
{

    private readonly ISecurityRepository _securityRepository;

    public SecurityRefreshResolver(ISecurityRepository securityRepository)
    {
        _securityRepository = securityRepository;
    }

    public async Task<Result<RefreshSessionContext>> ResolveAsync(string hashedRefreshToken, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _securityRepository.GetRefreshTokenByHashAsync(hashedRefreshToken, cancellationToken);

        if (refreshToken is null)
        {
            return Result.Failure<RefreshSessionContext>(Error.Failure("Auth.InvalidToken", "Token không hợp lệ."));
        }

        if (refreshToken.IsRevoked)
        {
            // Replay Attack Detection: Revoke entire family
            var familyTokens = await _securityRepository.GetRefreshTokensByFamilyAsync(refreshToken.FamilyId, cancellationToken);
            foreach (var token in familyTokens)
            {
                if (!token.IsRevoked)
                {
                    token.Revoke("Compromised: Token Replay");
                    _securityRepository.UpdateRefreshToken(token);
                }
            }

            return Result.Failure<RefreshSessionContext>(SessionErrors.Compromised);
        }

        if (refreshToken.IsExpired)
        {
            return Result.Failure<RefreshSessionContext>(SessionErrors.Expired);
        }

        if (!refreshToken.IsActive)
        {
            return Result.Failure<RefreshSessionContext>(Error.Failure("Auth.InvalidSession", "Phiên đăng nhập không hợp lệ hoặc đã bị thu hồi."));
        }

        var deviceSession = await _securityRepository.GetDeviceSessionByIdAsync(refreshToken.DeviceSessionId, cancellationToken);
        if (deviceSession == null || !deviceSession.IsActive())
        {
            return Result.Failure<RefreshSessionContext>(Error.Failure("Auth.InvalidSession", "Thiết bị không hợp lệ hoặc đã bị đăng xuất."));
        }

        return Result.Success(new RefreshSessionContext(refreshToken.CitizenIdentityId, refreshToken.DeviceSessionId.Value, refreshToken.FamilyId, refreshToken.Id.Value, refreshToken.UserId));
    }

    public async Task RotateAsync(RefreshSessionContext context, string newHashedRefreshToken, DateTime expiryDate, CancellationToken cancellationToken = default)
    {

        // 1. Revoke old refresh token
        if (context.OldRefreshTokenId.HasValue)
        {
            var oldToken = await _securityRepository.GetRefreshTokenAsync(context.OldRefreshTokenId.Value, cancellationToken);
            if (oldToken != null && !oldToken.IsRevoked)
            {
                oldToken.Revoke("Rotated");
                _securityRepository.UpdateRefreshToken(oldToken);
            }
        }

        // 2. Create new refresh token with same FamilyId
        var deviceSessionId = new DeviceSessionId(context.SessionId);
        var newRefreshToken = AnSinhSo.Domain.Aggregates.SecurityAggregate.RefreshToken.Create(
            context.UserId ?? Guid.Empty,
            context.CitizenIdentityId,
            newHashedRefreshToken,
            context.FamilyId,
            expiryDate,
            deviceSessionId);

        _securityRepository.AddRefreshToken(newRefreshToken);

        // 3. Update device session
        var deviceSession = await _securityRepository.GetDeviceSessionByIdAsync(deviceSessionId, cancellationToken);
        if (deviceSession != null)
        {
            deviceSession.LinkRefreshToken(newRefreshToken.Id.Value);
            deviceSession.UpdateLastSeen();
            _securityRepository.UpdateDeviceSession(deviceSession);
        }
    }
}
