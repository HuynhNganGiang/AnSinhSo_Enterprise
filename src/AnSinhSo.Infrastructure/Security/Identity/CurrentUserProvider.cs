using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using AnSinhSo.Application.Common.Security;

namespace AnSinhSo.Infrastructure.Security.Identity;

public sealed class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserInfo CurrentUser
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            
            if (user is null || !user.Identity?.IsAuthenticated == true)
            {
                return new CurrentUserInfo(Guid.Empty, string.Empty, string.Empty, Array.Empty<string>(), false, new ClaimsPrincipal());
            }

            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
                               user.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? 
                               Guid.Empty.ToString();
                               
            _ = Guid.TryParse(userIdString, out Guid userId);

            var username = user.FindFirstValue(ClaimTypes.Name) ?? 
                           user.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty;
                           
            var email = user.FindFirstValue(ClaimTypes.Email) ?? 
                        user.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty;

            var roles = new List<string>();
            foreach (var claim in user.Claims)
            {
                if (claim.Type == ClaimTypes.Role)
                {
                    roles.Add(claim.Value);
                }
            }

            return new CurrentUserInfo(
                userId,
                username,
                email,
                roles,
                true,
                user);
        }
    }
}

