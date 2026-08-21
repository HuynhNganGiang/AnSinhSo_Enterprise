using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace AnSinhSo.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly AuthenticationOptions _authOptions;

    public JwtProvider(
        IOptions<JwtOptions> jwtOptions,
        IOptions<AuthenticationOptions> authOptions)
    {
        _jwtOptions = jwtOptions.Value;
        _authOptions = authOptions.Value;
    }

    public string GenerateAccessToken(CitizenIdentity identity, UserSessionId userSessionId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Sid, userSessionId.Value.ToString()),
            new Claim("SessionType", "Citizen"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        // Hardening: Verify algorithm mapping or use a strong default
        var signingAlgorithm = _jwtOptions.SigningAlgorithm switch
        {
            "HS384" => SecurityAlgorithms.HmacSha384,
            "HS512" => SecurityAlgorithms.HmacSha512,
            _ => SecurityAlgorithms.HmacSha256 // Default and fallback
        };
        var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

        var now = DateTime.UtcNow;

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            now, // nbf
            now.AddMinutes(_authOptions.AccessTokenLifetimeMinutes), // exp
            signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessTokenForUser(AnSinhSo.Domain.Aggregates.UserAggregate.User user, AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId deviceSessionId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Sid, deviceSessionId.Value.ToString()),
            new Claim("SessionType", "BackOffice"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        // Hardening: Verify algorithm mapping or use a strong default
        var signingAlgorithm = _jwtOptions.SigningAlgorithm switch
        {
            "HS384" => SecurityAlgorithms.HmacSha384,
            "HS512" => SecurityAlgorithms.HmacSha512,
            _ => SecurityAlgorithms.HmacSha256 // Default and fallback
        };
        var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

        var now = DateTime.UtcNow;

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            now, // nbf
            now.AddMinutes(_authOptions.AccessTokenLifetimeMinutes), // exp
            signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
