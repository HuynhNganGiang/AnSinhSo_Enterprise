using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AnSinhSo.API.Controllers;

[ApiController]
[Route("api/v1/system/configuration-status")]
[Authorize(Roles = "Admin")]
public sealed class SystemConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly AnSinhSoDbContext _dbContext;

    public SystemConfigurationController(
        IConfiguration configuration,
        IHostEnvironment environment,
        AnSinhSoDbContext dbContext)
    {
        _configuration = configuration;
        _environment = environment;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var databaseName =
            _dbContext.Database
                .GetDbConnection()
                .Database;

        var provider =
            _dbContext.Database.ProviderName ??
            "Unknown";

        var connectionStrings =
            _configuration
                .GetSection("ConnectionStrings");

        var jwt =
            _configuration
                .GetSection("Jwt");

        var zalo =
            GetFirstSection(
                "ZaloOA",
                "Zalo");

        var sms =
            GetFirstSection(
                "Sms",
                "SMS");

        var demoValue =
            ReadDemoData();

        var allowedHosts =
            _configuration["AllowedHosts"];

        return Ok(
            new
            {
                success = true,

                data = new
                {
                    source =
                        "REAL RUNTIME",

                    application =
                        "AnSinhSo Enterprise",

                    api =
                        ".NET 8 / ASP.NET Core Web API",

                    environment =
                        _environment.EnvironmentName,

                    generatedAt =
                        DateTimeOffset.UtcNow,

                    database = new
                    {
                        provider,

                        name =
                            databaseName,

                        isRealDatabase =
                            string.Equals(
                                databaseName,
                                "AnSinhSoRealDb",
                                StringComparison.OrdinalIgnoreCase),

                        configured =
                            HasConfiguredValue(
                                connectionStrings)
                    },

                    demoData = new
                    {
                        configured =
                            demoValue.HasValue,

                        enabled =
                            demoValue
                    },

                    jwt = new
                    {
                        sectionPresent =
                            SectionExists(jwt),

                        configured =
                            HasConfiguredValue(jwt),

                        signingMaterialConfigured =
                            HasSensitiveValue(jwt),

                        configuredEntries =
                            CountConfiguredValues(jwt)
                    },

                    zaloOA = new
                    {
                        sectionPresent =
                            SectionExists(zalo),

                        configured =
                            HasConfiguredValue(zalo),

                        appIdConfigured =
                            HasAnyValue(
                                zalo,
                                "AppId",
                                "AppID"),

                        appSecretConfigured =
                            HasAnyValue(
                                zalo,
                                "AppSecret",
                                "Secret"),

                        accessTokenConfigured =
                            HasAnyValue(
                                zalo,
                                "AccessToken"),

                        refreshTokenConfigured =
                            HasAnyValue(
                                zalo,
                                "RefreshToken"),

                        configuredEntries =
                            CountConfiguredValues(zalo)
                    },

                    sms = new
                    {
                        sectionPresent =
                            SectionExists(sms),

                        configured =
                            HasConfiguredValue(sms),

                        configuredEntries =
                            CountConfiguredValues(sms)
                    },

                    allowedHostsConfigured =
                        !string.IsNullOrWhiteSpace(
                            allowedHosts),

                    security = new
                    {
                        secretsReturned =
                            false,

                        connectionStringReturned =
                            false,

                        tokensReturned =
                            false,

                        readOnlyEndpoint =
                            true
                    }
                }
            });
    }


    private bool? ReadDemoData()
    {
        var direct =
            _configuration["DemoData"];

        if (
            bool.TryParse(
                direct,
                out var directResult))
        {
            return directResult;
        }

        var nested =
            _configuration[
                "DemoData:Enabled"];

        if (
            bool.TryParse(
                nested,
                out var nestedResult))
        {
            return nestedResult;
        }

        return null;
    }


    private IConfigurationSection GetFirstSection(
        params string[] names)
    {
        foreach (var name in names)
        {
            var section =
                _configuration
                    .GetSection(name);

            if (SectionExists(section))
            {
                return section;
            }
        }

        return
            _configuration
                .GetSection(names[0]);
    }


    private static bool SectionExists(
        IConfigurationSection section)
    {
        return
            section.Value is not null ||
            section.GetChildren().Any();
    }


    private static bool HasAnyValue(
        IConfigurationSection section,
        params string[] keys)
    {
        foreach (var key in keys)
        {
            if (
                !string.IsNullOrWhiteSpace(
                    section[key]))
            {
                return true;
            }
        }

        return false;
    }


    private static bool HasConfiguredValue(
        IConfigurationSection section)
    {
        if (
            !string.IsNullOrWhiteSpace(
                section.Value))
        {
            return true;
        }

        return
            section
                .AsEnumerable()
                .Any(
                    item =>
                        item.Key !=
                            section.Path &&
                        !string.IsNullOrWhiteSpace(
                            item.Value));
    }


    private static int CountConfiguredValues(
        IConfigurationSection section)
    {
        var direct =
            string.IsNullOrWhiteSpace(
                section.Value)
                ? 0
                : 1;

        return
            direct +
            section
                .AsEnumerable()
                .Count(
                    item =>
                        item.Key !=
                            section.Path &&
                        !string.IsNullOrWhiteSpace(
                            item.Value));
    }


    private static bool HasSensitiveValue(
        IConfigurationSection section)
    {
        return
            section
                .AsEnumerable()
                .Any(
                    item =>
                    {
                        if (
                            string.IsNullOrWhiteSpace(
                                item.Value))
                        {
                            return false;
                        }

                        var key =
                            item.Key
                                .ToLowerInvariant();

                        return
                            key.Contains(
                                "secret") ||
                            key.Contains(
                                "signing") ||
                            key.EndsWith(
                                ":key") ||
                            key.Contains(
                                "password");
                    });
    }
}