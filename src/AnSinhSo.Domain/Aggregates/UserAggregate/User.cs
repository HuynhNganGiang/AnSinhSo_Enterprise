using System;
using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{
    private readonly List<RefreshToken> _refreshTokens = new();

    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string SecurityStamp { get; private set; } = string.Empty;

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() {} // ORM

    // Optional: Constructor to create a new User, but not strictly needed if we only deal with Login here.
    // If we need to create one, we would do it here.

    public void IssueRefreshToken(
        string tokenHash, 
        DateTime expiresAtUtc, 
        string createdByIp, 
        string deviceName, 
        string userAgent, 
        string jwtId)
    {
        var refreshToken = new RefreshToken(
            tokenHash,
            DateTime.UtcNow,
            expiresAtUtc,
            createdByIp,
            deviceName,
            userAgent,
            jwtId);

        _refreshTokens.Add(refreshToken);
    }

    public void RotateRefreshToken(
        string oldTokenHash, 
        string newTokenHash, 
        DateTime expiresAtUtc, 
        string createdByIp, 
        string deviceName, 
        string userAgent, 
        string jwtId)
    {
        var oldToken = _refreshTokens.SingleOrDefault(t => t.TokenHash == oldTokenHash);
        
        if (oldToken is null) return;
        
        if (oldToken.IsRevoked)
        {
            // Token reuse detection logic -> Revoke all tokens if a revoked token is used
            RevokeAllRefreshTokens(createdByIp);
            return;
        }

        oldToken.Revoke(DateTime.UtcNow, createdByIp, newTokenHash);

        IssueRefreshToken(newTokenHash, expiresAtUtc, createdByIp, deviceName, userAgent, jwtId);
    }

    public void RevokeRefreshToken(string tokenHash, string revokedByIp)
    {
        var token = _refreshTokens.SingleOrDefault(t => t.TokenHash == tokenHash);
        
        if (token is not null && token.IsActive(DateTime.UtcNow))
        {
            token.Revoke(DateTime.UtcNow, revokedByIp);
        }
    }

    public void RevokeAllRefreshTokens(string revokedByIp)
    {
        var utcNow = DateTime.UtcNow;
        foreach (var token in _refreshTokens.Where(t => t.IsActive(utcNow)))
        {
            token.Revoke(utcNow, revokedByIp);
        }
    }

    public void ChangePasswordHash(string newHash)
    {
        PasswordHash = newHash;
        UpdateSecurityStamp();
    }

    public void UpdateSecurityStamp()
    {
        SecurityStamp = Guid.NewGuid().ToString("N");
    }
}
