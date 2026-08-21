using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authentication.BackOffice.Login;
using AnSinhSo.Application.Authentication.BackOffice.BackOfficeLogin;
using AnSinhSo.Application.Authentication.BackOffice.Logout;
using AnSinhSo.Application.Authentication.BackOffice.LogoutAllSessions;
using AnSinhSo.Application.Authentication.BackOffice.RefreshToken;
using AnSinhSo.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;
using AnSinhSo.API.Controllers;

namespace AnSinhSo.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthenticationController : ApiControllerBase
{
    private readonly ISender _sender;

    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [EnableRateLimiting("LoginPolicy")]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AnSinhSo.Application.Authentication.AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = "unknown";

        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        if (string.IsNullOrEmpty(userAgent)) userAgent = "unknown";

        var command = new LoginCommand(request.PhoneNumber, request.OtpCode, ipAddress, userAgent, request.DeviceName);
        
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error.Code.Contains("Unauthorized") || result.Error.Code.Contains("Invalid"))
            {
                return Unauthorized(result.Error);
            }
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [EnableRateLimiting("LoginPolicy")]
    [HttpPost("backoffice/login")]
    [ProducesResponseType(typeof(AnSinhSo.Application.Authentication.AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> BackOfficeLogin([FromBody] BackOfficeLoginRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = "unknown";

        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        if (string.IsNullOrEmpty(userAgent)) userAgent = "unknown";

        var command = new BackOfficeLoginCommand(request.Username, request.Password, ipAddress, userAgent, request.DeviceName, false, "vi-VN", "UTC");
        
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error.Code.Contains("Unauthorized") || result.Error.Code.Contains("Invalid"))
            {
                return Unauthorized(result.Error);
            }
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [EnableRateLimiting("RefreshPolicy")]
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AnSinhSo.Application.Authentication.AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(ipAddress)) ipAddress = "unknown";

        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        if (string.IsNullOrEmpty(userAgent)) userAgent = "unknown";

        var command = new RefreshTokenCommand(request.RefreshToken, ipAddress, userAgent, request.DeviceName);
        
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error.Code.Contains("Unauthorized") || 
                result.Error.Code.Contains("Invalid") || 
                result.Error.Code.StartsWith("Session."))
            {
                return Unauthorized(result.Error);
            }
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var sidClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value 
            ?? HttpContext.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid)?.Value;
        
        if (!Guid.TryParse(sidClaim, out var sessionId))
        {
            var claims = string.Join(", ", HttpContext.User.Claims.Select(c => c.Type + "=" + c.Value));
            throw new Exception($"Unauthorized. Claims: {claims}");
        }

        var command = new LogoutCommand(sessionId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [Authorize]
    [EnableRateLimiting("LogoutAllPolicy")]
    [HttpPost("logout-all-sessions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogoutAllSessions(CancellationToken cancellationToken)
    {
        var subClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
            ?? HttpContext.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
        
        if (!Guid.TryParse(subClaim, out var citizenIdentityId))
        {
            return Unauthorized();
        }

        var command = new LogoutAllSessionsCommand(citizenIdentityId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}
