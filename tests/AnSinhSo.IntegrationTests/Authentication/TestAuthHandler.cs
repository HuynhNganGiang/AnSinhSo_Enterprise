using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AnSinhSo.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnSinhSo.IntegrationTests.Authentication;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string TestScheme = "TestScheme";

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var headerValue = authHeader.ToString();
            
            // "Bearer SystemAdmin"
            if (headerValue == "Bearer SystemAdmin")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "00000000-0000-0000-0000-000000000001"),
                    new Claim(ClaimTypes.Name, "System Admin"),
                    new Claim("permissions", Permissions.Roles.View),
                    new Claim("permissions", Permissions.Roles.Create),
                    new Claim("permissions", Permissions.Roles.Update),
                    new Claim("permissions", Permissions.Roles.Delete),
                    new Claim("permissions", Permissions.PermissionsModule.View),
                    new Claim("permissions", Permissions.PermissionsModule.Manage),
                    new Claim("permissions", Permissions.UserRoles.View),
                    new Claim("permissions", Permissions.UserRoles.Manage),
                    new Claim("permissions", Permissions.Households.Read),
                    new Claim("permissions", Permissions.Households.Create),
                    new Claim("permissions", Permissions.Households.Update),
                    new Claim("permissions", Permissions.Households.Delete),
                    new Claim("permissions", Permissions.WelfarePrograms.View),
                    new Claim("permissions", Permissions.WelfarePrograms.Create),
                    new Claim("permissions", Permissions.WelfarePrograms.Update),
                    new Claim("permissions", Permissions.WelfareCases.View),
                    new Claim("permissions", Permissions.WelfareCases.Create),
                    new Claim("permissions", Permissions.WelfareCases.Update),
                    new Claim("permissions", Permissions.WelfareCases.Decide),
                    new Claim("permissions", Permissions.WelfareCases.Cancel),
                    new Claim("permissions", Permissions.WelfareCases.Close)
                };
                var identity = new ClaimsIdentity(claims, TestScheme);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, TestScheme);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            
            // "Bearer BasicUser"
            if (headerValue == "Bearer BasicUser")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "00000000-0000-0000-0000-000000000002"),
                    new Claim(ClaimTypes.Name, "Basic User"),
                    new Claim("permissions", Permissions.Roles.View) // Only view
                };
                var identity = new ClaimsIdentity(claims, TestScheme);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, TestScheme);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
