using System.Data;
using System.Linq;
using AnSinhSo.Domain.Constants;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AnSinhSo.API.Controllers;

[ApiController]
[Route("api/v1/system/integration-status")]
[Authorize(
    Policy = Permissions.PermissionsModule.View)]
public sealed class SystemIntegrationController : ControllerBase
{
    private const string ProductionDatabase =
        "AnSinhSoRealDb";

    private readonly AnSinhSoDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public SystemIntegrationController(
        AnSinhSoDbContext dbContext,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatus(
        CancellationToken cancellationToken)
    {
        var runtimeDatabase =
            GetRuntimeDatabase();

        var counts =
            await GetCountsAsync(
                cancellationToken);

        var zalo =
            _configuration.GetSection(
                "ZaloOA");

        var sms =
            _configuration.GetSection(
                "SmsProvider");

        var zaloKeys =
            new[]
            {
                "AppId",
                "AppSecret",
                "AccessToken",
                "RefreshToken"
            };

        var smsKeys =
            new[]
            {
                "ApiUrl",
                "ApiKey",
                "SenderId"
            };

        var zaloConfigured =
            zaloKeys.Count(
                key =>
                    HasValue(
                        zalo[key]));

        var smsConfigured =
            smsKeys.Count(
                key =>
                    HasValue(
                        sms[key]));

        return Ok(
            new
            {
                success = true,

                data = new
                {
                    source =
                        "REAL RUNTIME + SOURCE TRUTH",

                    generatedAt =
                        DateTimeOffset.UtcNow,

                    database = new
                    {
                        runtimeDatabase,

                        productionDatabase =
                            ProductionDatabase,

                        isProductionRuntime =
                            string.Equals(
                                runtimeDatabase,
                                ProductionDatabase,
                                StringComparison.OrdinalIgnoreCase)
                    },

                    zaloOA = new
                    {
                        implementation =
                            "REAL API IMPLEMENTATION",

                        liveConnection =
                            "NOT VERIFIED",

                        liveConnectionVerified =
                            false,

                        sectionName =
                            "ZaloOA",

                        sectionPresent =
                            zalo.GetChildren().Any(),

                        configuredEntries =
                            zaloConfigured,

                        expectedEntries =
                            zaloKeys.Length,

                        appIdConfigured =
                            HasValue(
                                zalo["AppId"]),

                        appSecretConfigured =
                            HasValue(
                                zalo["AppSecret"]),

                        accessTokenConfigured =
                            HasValue(
                                zalo["AccessToken"]),

                        refreshTokenConfigured =
                            HasValue(
                                zalo["RefreshToken"]),

                        externalNetworkCallPerformed =
                            false
                    },

                    notificationDelivery = new
                    {
                        implementation =
                            "MOCK",

                        externalZaloDelivery =
                            false,

                        simulatedSuccess =
                            true,

                        warning =
                            "ZaloNotificationService currently simulates delivery and returns success."
                    },

                    webhook = new
                    {
                        implementation =
                            "IMPLEMENTED",

                        route =
                            "/api/v1/zalo/webhook",

                        signatureValidation =
                            true,

                        liveWebhookVerified =
                            false
                    },

                    smsOtp = new
                    {
                        implementation =
                            "HTTP PROVIDER IMPLEMENTED",

                        sectionName =
                            "SmsProvider",

                        sectionPresent =
                            sms.GetChildren().Any(),

                        configuredEntries =
                            smsConfigured,

                        expectedEntries =
                            smsKeys.Length,

                        configured =
                            smsConfigured ==
                            smsKeys.Length,

                        liveDeliveryVerified =
                            false
                    },

                    productionData = new
                    {
                        zaloUsers =
                            counts.ZaloUsers,

                        notifications =
                            counts.Notifications,

                        notificationHistories =
                            counts.NotificationHistories
                    },

                    security = new
                    {
                        secretsReturned =
                            false,

                        tokensReturned =
                            false,

                        credentialsReturned =
                            false,

                        externalCallsPerformed =
                            false,

                        readOnlyEndpoint =
                            true
                    }
                }
            });
    }

    private string GetRuntimeDatabase()
    {
        var source =
            _dbContext
                .Database
                .GetDbConnection()
                .ConnectionString;

        if (
            string.IsNullOrWhiteSpace(
                source))
        {
            return string.Empty;
        }

        var builder =
            new SqlConnectionStringBuilder(
                source);

        return builder.InitialCatalog;
    }

    private async Task<ProductionCounts>
        GetCountsAsync(
            CancellationToken cancellationToken)
    {
        var source =
            _dbContext
                .Database
                .GetDbConnection()
                .ConnectionString;

        if (
            string.IsNullOrWhiteSpace(
                source))
        {
            throw new InvalidOperationException(
                "Runtime SQL connection is unavailable.");
        }

        var builder =
            new SqlConnectionStringBuilder(
                source)
            {
                InitialCatalog =
                    ProductionDatabase
            };

        await using var connection =
            new SqlConnection(
                builder.ConnectionString);

        await connection.OpenAsync(
            cancellationToken);

        await using var command =
            connection.CreateCommand();

        command.CommandText =
"""
SELECT
    (SELECT COUNT_BIG(*) FROM dbo.ZaloUsers),
    (SELECT COUNT_BIG(*) FROM dbo.Notifications),
    (SELECT COUNT_BIG(*) FROM dbo.NotificationHistories);
""";

        await using var reader =
            await command.ExecuteReaderAsync(
                CommandBehavior.SingleRow,
                cancellationToken);

        if (
            !await reader.ReadAsync(
                cancellationToken))
        {
            throw new InvalidOperationException(
                "Integration counts are unavailable.");
        }

        return new ProductionCounts(
            reader.GetInt64(0),
            reader.GetInt64(1),
            reader.GetInt64(2));
    }

    private static bool HasValue(
        string? value)
    {
        return
            !string.IsNullOrWhiteSpace(
                value);
    }

    private sealed record ProductionCounts(
        long ZaloUsers,
        long Notifications,
        long NotificationHistories);
}