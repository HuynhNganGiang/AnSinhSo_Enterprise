using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Net.Http.Headers;

namespace AnSinhSo.IntegrationTests.Authentication;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public bool UseMockAuthentication { get; set; } = true;

    public CustomWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("Jwt__SecretKey", "SuperSecretKeyForIntegrationTestingThatIsAtLeast32BytesLongSoItPassesValidation12345");
        Environment.SetEnvironmentVariable("Jwt__Issuer", "TestIssuer");
        Environment.SetEnvironmentVariable("Jwt__Audience", "TestAudience");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AnSinhSoDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var dbName = $"InMemoryDbForTesting_{Guid.NewGuid()}";
            services.AddDbContext<AnSinhSoDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
            });

            if (UseMockAuthentication)
            {
                // Mock Authentication
                services.AddAuthentication(TestAuthHandler.TestScheme)
                    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                        TestAuthHandler.TestScheme, options => { });
            }

            foreach (var s in services) {
                if (s.ServiceType.Name.Contains("IHostedService")) {
                    System.Console.WriteLine("DEBUG: HostedService registered -> " + (s.ImplementationType?.FullName ?? "null"));
                }
            }

            // Remove existing IDistributedCache
            var cacheDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Microsoft.Extensions.Caching.Distributed.IDistributedCache));
            if (cacheDescriptor != null)
            {
                services.Remove(cacheDescriptor);
            }
            services.AddDistributedMemoryCache();

            // Mock Permission Resolver
            var permissionResolverDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AnSinhSo.Application.Authorization.Abstractions.IPermissionResolver));
            if (permissionResolverDescriptor != null)
            {
                services.Remove(permissionResolverDescriptor);
            }
            services.AddScoped<AnSinhSo.Application.Authorization.Abstractions.IPermissionResolver, TestPermissionResolver>();

            // Ensure schema is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AnSinhSoDbContext>();
            db.Database.EnsureCreated();
        });

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Jwt:SecretKey", "SuperSecretKeyForIntegrationTestingThatIsAtLeast32BytesLongSoItPassesValidation12345" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "ConnectionStrings:Redis", "" }
            };
            configBuilder.AddInMemoryCollection(dict!);
        });
    }
}

public class TestPermissionResolver : AnSinhSo.Application.Authorization.Abstractions.IPermissionResolver
{
    private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

    public TestPermissionResolver(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<System.Collections.Generic.IReadOnlyCollection<string>> GetPermissionsAsync(AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId citizenIdentityId, System.Threading.CancellationToken cancellationToken = default)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Task.FromResult<System.Collections.Generic.IReadOnlyCollection<string>>(System.Array.Empty<string>());
        }

        var permissions = user.Claims
            .Where(c => c.Type == "permissions")
            .Select(c => c.Value)
            .ToList();

        return Task.FromResult<System.Collections.Generic.IReadOnlyCollection<string>>(permissions);
    }
}
