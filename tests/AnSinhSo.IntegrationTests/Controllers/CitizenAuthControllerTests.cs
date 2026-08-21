using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Authentication.Citizen.RequestOtp;
using AnSinhSo.Application.Authentication.Citizen.VerifyOtp;
using AnSinhSo.IntegrationTests.Authentication;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace AnSinhSo.IntegrationTests.Controllers;

public class CitizenAuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly System.Net.Http.HttpClient _client;

    public CitizenAuthControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async System.Threading.Tasks.Task SeedDataAsync(System.Action<AnSinhSo.Infrastructure.Persistence.Contexts.AnSinhSoDbContext, System.IServiceProvider> seedAction)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSo.Infrastructure.Persistence.Contexts.AnSinhSoDbContext>();
        seedAction(db, scope.ServiceProvider);

        foreach (var entry in db.ChangeTracker.Entries().Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified))
        {
            var rowVersionProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "RowVersion");
            if (rowVersionProp != null)
            {
                rowVersionProp.CurrentValue = System.Guid.NewGuid().ToByteArray().Take(8).ToArray();
            }
        }

        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task RequestOtp_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var phone = "0987654321";
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var identity = AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentity.Create(
                new AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId(System.Guid.NewGuid()), 
                new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(System.Guid.NewGuid()), 
                System.Guid.NewGuid().ToString(), 
                AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects.PhoneNumber.Create(phone));
            identity.VerifyPhoneNumber(AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects.PhoneNumber.Create(phone), System.DateTime.UtcNow);
            db.CitizenIdentities.Add(identity);
        });

        var command = new RequestCitizenOtpCommand(phone);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/citizen/request-otp", command);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task RequestOtp_ShouldReturnBadRequest_WhenPhoneNumberIsInvalid()
    {
        // Arrange
        var command = new RequestCitizenOtpCommand("invalid");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/citizen/request-otp", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task VerifyOtp_ShouldReturnBadRequest_WhenOtpIsInvalid()
    {
        // Arrange
        var command = new VerifyCitizenOtpCommand(System.Guid.NewGuid(), "000000", "Dev", "Chrome", "Win", "PC", "1.1.1.1", "fp", "UTC", false);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/citizen/verify-otp", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
