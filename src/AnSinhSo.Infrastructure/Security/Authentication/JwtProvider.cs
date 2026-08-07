using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AnSinhSo.Application.Common.Security;
using AnSinhSo.Domain.Aggregates.UserAggregate;

namespace AnSinhSo.Infrastructure.Security.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly TimeProvider _timeProvider;

    public JwtProvider(IOptions<JwtOptions> options, TimeProvider timeProvider)
    {
        _options = options.Value;
        _timeProvider = timeProvider;
    }

    public TokenResult Generate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // If roles were present, they would be added here
        // claims.Add(new Claim(ClaimTypes.Role, "RoleName"));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(securityKey, _options.SigningAlgorithm);

        var expires = _timeProvider.GetUtcNow().AddMinutes(_options.AccessTokenMinutes).UtcDateTime;

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            expires,
            credentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        // Find the JTI claim to return
        var jwtId = claims.Find(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;

        return new TokenResult(tokenValue, expires, jwtId, _options.RefreshTokenDays);
    }
}
