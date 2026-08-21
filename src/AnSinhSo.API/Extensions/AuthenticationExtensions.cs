using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AnSinhSo.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secret = jwtSettings["SecretKey"];
        
        if (string.IsNullOrEmpty(secret))
        {
            throw new System.Exception("JWT Secret is missing in appsettings.json");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = System.TimeSpan.Zero
            };
            
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var principal = context.Principal;
                    if (principal == null) return;
                    
                    var sessionType = principal.FindFirst("SessionType")?.Value;
                    var tokenSecurityStamp = principal.FindFirst("SecurityStamp")?.Value;
                    var subStr = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                        ?? principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
                        
                    if (string.IsNullOrEmpty(subStr) || !System.Guid.TryParse(subStr, out var id))
                    {
                        context.Fail("Invalid Subject.");
                        return;
                    }
                    
                    var sidStr = principal.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value 
                        ?? principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid)?.Value;
                        
                    if (string.IsNullOrEmpty(sidStr) || !System.Guid.TryParse(sidStr, out var sid))
                    {
                        context.Fail("Invalid Session.");
                        return;
                    }

                    if (sessionType == "BackOffice")
                    {
                        var sessionRepo = context.HttpContext.RequestServices.GetRequiredService<AnSinhSo.Domain.Interfaces.ISecurityRepository>();
                        var deviceSession = await sessionRepo.GetDeviceSessionByIdAsync(new AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId(sid), context.HttpContext.RequestAborted);
                        
                        if (deviceSession == null || !deviceSession.IsActive())
                        {
                            context.Fail("Session is revoked or not found.");
                            return;
                        }

                        var userRepo = context.HttpContext.RequestServices.GetRequiredService<AnSinhSo.Domain.Interfaces.IUserRepository>();
                        var user = await userRepo.GetByIdAsync(new AnSinhSo.Domain.Aggregates.UserAggregate.UserId(deviceSession.UserId), context.HttpContext.RequestAborted);
                        
                        if (user == null || user.IsLocked)
                        {
                            context.Fail("User account is not active or locked.");
                            return;
                        }
                        
                        // Check if security stamp has changed (e.g. password changed)
                        if (deviceSession.SecurityStamp != user.SecurityStamp)
                        {
                            context.Fail("Security stamp changed.");
                            return;
                        }
                    }
                    else if (sessionType == "Citizen")
                    {
                        var sessionRepo = context.HttpContext.RequestServices.GetRequiredService<AnSinhSo.Domain.Interfaces.IUserSessionRepository>();
                        var session = await sessionRepo.GetByIdAsync(new AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects.UserSessionId(sid), context.HttpContext.RequestAborted);
                        
                        if (session == null || session.IsRevoked)
                        {
                            context.Fail("Session is revoked or not found.");
                            return;
                        }

                        var citizenRepo = context.HttpContext.RequestServices.GetRequiredService<AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ICitizenIdentityRepository>();
                        var citizen = await citizenRepo.GetByIdAsync(new AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId(id), context.HttpContext.RequestAborted);
                        
                        if (citizen == null || citizen.Status != AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Verified)
                        {
                            context.Fail("Citizen is not active or not found.");
                            return;
                        }
                    }
                }
            };
        });

        return services;
    }
}
