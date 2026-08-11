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
    public CustomWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("Authentication__SecretKey", "SuperSecretKeyForIntegrationTestingThatIsAtLeast32BytesLongSoItPassesValidation12345");
        Environment.SetEnvironmentVariable("Authentication__Issuer", "TestIssuer");
        Environment.SetEnvironmentVariable("Authentication__Audience", "TestAudience");
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

            services.AddDbContext<AnSinhSoDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

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
                { "Authentication:SecretKey", "SuperSecretKeyForIntegrationTestingThatIsAtLeast32BytesLongSoItPassesValidation12345" },
                { "Authentication:Issuer", "TestIssuer" },
                { "Authentication:Audience", "TestAudience" }
            };
            configBuilder.AddInMemoryCollection(dict!);
        });
    }
}
