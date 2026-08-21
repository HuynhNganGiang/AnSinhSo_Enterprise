using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Authentication.Citizen;
using AnSinhSo.Application.Authentication.Citizen.RequestOtp;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.API.Filters;

public class CitizenOtpAntiSpamFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var rateLimitService = context.HttpContext.RequestServices.GetRequiredService<IOtpRateLimitService>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<CitizenOtpAntiSpamFilter>>();
        
        string? phoneNumber = null;
        if (context.ActionArguments.TryGetValue("command", out var cmdObj) && cmdObj is RequestCitizenOtpCommand cmd)
        {
            phoneNumber = cmd.PhoneNumber;
        }

        if (string.IsNullOrEmpty(phoneNumber))
        {
            await next();
            return;
        }

        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip";
        var deviceFingerprint = context.HttpContext.Request.Headers["X-Device-Fingerprint"].ToString();
        if (string.IsNullOrEmpty(deviceFingerprint))
        {
            deviceFingerprint = "unknown_device";
        }

        var result = await rateLimitService.CheckRateLimitAsync(phoneNumber, ip, deviceFingerprint);

        if (!result.Allowed)
        {
            await HandleRateLimitExceeded(context, phoneNumber, ip, deviceFingerprint, result);
            return;
        }

        // Proceed
        var executedContext = await next();

        // If OTP requested successfully, increment counters and set cooldown
        if (executedContext.Exception == null && executedContext.Result is ObjectResult objResult)
        {
            if (objResult.StatusCode == 200)
            {
                await rateLimitService.RecordSuccessAsync(phoneNumber, ip, deviceFingerprint);
            }
        }
    }

    private async Task HandleRateLimitExceeded(ActionExecutingContext context, string phoneNumber, string ip, string deviceFingerprint, Application.Abstractions.Authentication.RateLimiting.RateLimitResult result)
    {
        var auditService = context.HttpContext.RequestServices.GetRequiredService<ISecurityAuditService>();
        await auditService.LogRateLimitExceededAsync(phoneNumber, ip, deviceFingerprint, result.Reason);

        var error = Error.TooManyRequests("Otp.RateLimitExceeded", result.Reason);
        var problemDetails = new ProblemDetails
        {
            Status = Microsoft.AspNetCore.Http.StatusCodes.Status429TooManyRequests,
            Title = "Too Many Requests",
            Detail = error.Message,
            Type = "https://tools.ietf.org/html/rfc6585#section-4",
            Extensions = { { "errors", new[] { error.Code } } }
        };

        context.HttpContext.Response.Headers["Retry-After"] = result.RetryAfterSeconds.ToString();
        context.HttpContext.Response.Headers["X-RateLimit-Limit"] = result.Limit.ToString();
        context.HttpContext.Response.Headers["X-RateLimit-Remaining"] = result.Remaining.ToString();
        context.HttpContext.Response.Headers["X-RateLimit-Reset"] = result.ResetUnixTimeSeconds.ToString();

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status429TooManyRequests
        };
    }
}
