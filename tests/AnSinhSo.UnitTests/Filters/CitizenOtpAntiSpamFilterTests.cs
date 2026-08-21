using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnSinhSo.API.Filters;
using AnSinhSo.Application.Authentication.Citizen;
using AnSinhSo.Application.Authentication.Citizen.RequestOtp;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Authentication.RateLimiting;
using AnSinhSo.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AnSinhSo.UnitTests.Filters;

public class CitizenOtpAntiSpamFilterTests
{
    private readonly Mock<IOtpRateLimitService> _rateLimitServiceMock;
    private readonly Mock<ISecurityAuditService> _securityAuditServiceMock;
    private readonly Mock<ILogger<CitizenOtpAntiSpamFilter>> _loggerMock;
    private readonly Mock<IUnitOfWork> _uowMock;

    public CitizenOtpAntiSpamFilterTests()
    {
        _rateLimitServiceMock = new Mock<IOtpRateLimitService>();
        _securityAuditServiceMock = new Mock<ISecurityAuditService>();
        _loggerMock = new Mock<ILogger<CitizenOtpAntiSpamFilter>>();
        _uowMock = new Mock<IUnitOfWork>();
    }

    private ActionExecutingContext CreateContext(RequestCitizenOtpCommand command, string ip = "127.0.0.1", string fingerprint = "test-device")
    {
        var services = new ServiceCollection();
        services.AddSingleton(_rateLimitServiceMock.Object);
        services.AddSingleton(_securityAuditServiceMock.Object);
        services.AddSingleton(_loggerMock.Object);
        services.AddSingleton(_loggerMock.Object);
        var serviceProvider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse(ip);
        httpContext.Request.Headers["X-Device-Fingerprint"] = fingerprint;

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor()
        );

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?> { { "command", command } },
            new object()
        );
    }

    [Fact]
    public async Task OnActionExecutionAsync_WhenNoCooldown_ShouldProceed()
    {
        // Arrange
        var filter = new CitizenOtpAntiSpamFilter();
        var command = new RequestCitizenOtpCommand("0123456789");
        var context = CreateContext(command);
        
        _rateLimitServiceMock.Setup(s => s.CheckRateLimitAsync("0123456789", "127.0.0.1", "test-device", It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(RateLimitResult.Success(5, 4, 1600000000));

        var nextCalled = false;
        Task<ActionExecutedContext> Next()
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(context, new List<IFilterMetadata>(), new object())
            {
                Result = new OkObjectResult(Guid.NewGuid())
            });
        }

        // Act
        await filter.OnActionExecutionAsync(context, Next);

        // Assert
        Assert.True(nextCalled);
        _rateLimitServiceMock.Verify(s => s.RecordSuccessAsync("0123456789", "127.0.0.1", "test-device", It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnActionExecutionAsync_WhenInCooldown_ShouldReturn429()
    {
        // Arrange
        var filter = new CitizenOtpAntiSpamFilter();
        var command = new RequestCitizenOtpCommand("0123456789");
        var context = CreateContext(command);
        
        var nextCalled = false;
        Task<ActionExecutedContext> Next()
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(context, new List<IFilterMetadata>(), new object())
            {
                Result = new OkObjectResult(Guid.NewGuid())
            });
        }

        _rateLimitServiceMock.Setup(s => s.CheckRateLimitAsync("0123456789", "127.0.0.1", "test-device", It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(RateLimitResult.Blocked(RateLimitType.Cooldown, RateLimitScope.Phone, "Cooldown active", 60, 5, 1600000000));

        // Act
        await filter.OnActionExecutionAsync(context, Next);

        // Assert
        Assert.False(nextCalled);
        Assert.IsType<ObjectResult>(context.Result);
        var result = (ObjectResult)context.Result;
        Assert.Equal(429, result.StatusCode);
        _securityAuditServiceMock.Verify(s => s.LogRateLimitExceededAsync("0123456789", "127.0.0.1", "test-device", "Cooldown active", It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }
}
