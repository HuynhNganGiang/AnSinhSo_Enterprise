using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Security;
using AnSinhSo.Contracts.Authentication;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.OtpVerificationAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Net.Http.Headers;

namespace AnSinhSo.IntegrationTests.Authentication;

public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthenticationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task SeedDataAsync(Action<AnSinhSoDbContext, IServiceProvider> seedAction)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSoDbContext>();
        seedAction(db, scope.ServiceProvider);

        foreach (var entry in db.ChangeTracker.Entries().Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified))
        {
            var rowVersionProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "RowVersion");
            if (rowVersionProp != null)
            {
                rowVersionProp.CurrentValue = Guid.NewGuid().ToByteArray().Take(8).ToArray();
            }
        }

        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task Login_ThànhCông()
    {
        // Arrange
        var phone = "0987654321";
        var rawOtp = "123456";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var hashProvider = sp.GetRequiredService<IHashProvider>();
            
            var identity = CitizenIdentity.Create(new CitizenIdentityId(Guid.NewGuid()), new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()), Guid.NewGuid().ToString(), PhoneNumber.Create(phone));
            identity.VerifyPhoneNumber(PhoneNumber.Create(phone), DateTime.UtcNow);
            db.CitizenIdentities.Add(identity);

            var hashedOtp = hashProvider.Hash(rawOtp);
            var otp = OtpVerification.Create(identity.Id, hashedOtp, PhoneNumber.Create(phone), DateTime.UtcNow.AddMinutes(5));
            db.OtpVerifications.Add(otp);
        });

        var request = new LoginRequest(phone, rawOtp, "TestDevice");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", request);

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, content);
        Assert.Contains("accessToken", content);
        Assert.Contains("refreshToken", content);
    }

    [Fact]
    public async Task Refresh_ReplayAttack()
    {
        // Arrange
        var phone = "0987654321";
        var rawOtp = "123456";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var hashProvider = sp.GetRequiredService<IHashProvider>();
            var identity = CitizenIdentity.Create(new CitizenIdentityId(Guid.NewGuid()), new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()), Guid.NewGuid().ToString(), PhoneNumber.Create(phone));
            identity.VerifyPhoneNumber(PhoneNumber.Create(phone), DateTime.UtcNow);
            db.CitizenIdentities.Add(identity);

            var hashedOtp = hashProvider.Hash(rawOtp);
            var otp = OtpVerification.Create(identity.Id, hashedOtp, PhoneNumber.Create(phone), DateTime.UtcNow.AddMinutes(5));
            db.OtpVerifications.Add(otp);
        });

        var loginRequest = new LoginRequest(phone, rawOtp, "TestDevice");
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        var loginResponseText = await loginResponse.Content.ReadAsStringAsync();
        var json = System.Text.Json.JsonDocument.Parse(loginResponseText);
        var refreshToken = json.RootElement.GetProperty("data").GetProperty("refreshToken").GetString();

        var refreshRequest = new RefreshTokenRequest(refreshToken!, "TestDevice");
        
        // Act 1: Use valid refresh token (Rotates to new one)
        var refreshResponse1 = await _client.PostAsJsonAsync("/api/v1/auth/refresh-token", refreshRequest);
        if (!refreshResponse1.IsSuccessStatusCode) throw new Exception("Refresh failed: " + await refreshResponse1.Content.ReadAsStringAsync());

        // Act 2: Use the SAME old refresh token (Replay Attack)
        var refreshResponse2 = await _client.PostAsJsonAsync("/api/v1/auth/refresh-token", refreshRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, refreshResponse2.StatusCode);
    }

    [Fact]
    public async Task Logout_SauĐóGọiApiProtected()
    {
        // Arrange
        var phone = "0987654321";
        var rawOtp = "123456";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var hashProvider = sp.GetRequiredService<IHashProvider>();
            var identity = CitizenIdentity.Create(new CitizenIdentityId(Guid.NewGuid()), new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()), Guid.NewGuid().ToString(), PhoneNumber.Create(phone));
            identity.VerifyPhoneNumber(PhoneNumber.Create(phone), DateTime.UtcNow);
            db.CitizenIdentities.Add(identity);

            var hashedOtp = hashProvider.Hash(rawOtp);
            var otp = OtpVerification.Create(identity.Id, hashedOtp, PhoneNumber.Create(phone), DateTime.UtcNow.AddMinutes(5));
            db.OtpVerifications.Add(otp);
        });

        var loginRequest = new LoginRequest(phone, rawOtp, "TestDevice");
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        var loginResponseText = await loginResponse.Content.ReadAsStringAsync();
        var json = System.Text.Json.JsonDocument.Parse(loginResponseText);
        var accessToken = json.RootElement.GetProperty("data").GetProperty("accessToken").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Act 1: Logout
        var logoutResponse = await _client.PostAsync("/api/v1/auth/logout", null);
        if (!logoutResponse.IsSuccessStatusCode) throw new Exception("Logout failed: " + await logoutResponse.Content.ReadAsStringAsync());

        // Act 2: Call protected endpoint again (or logout again)
        var protectedCallResponse = await _client.PostAsync("/api/v1/auth/logout", null);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, protectedCallResponse.StatusCode);
    }

    [Fact]
    public async Task CitizenDisable_JwtCònHạn()
    {
        // Arrange
        var phone = "0987654321";
        var rawOtp = "123456";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var hashProvider = sp.GetRequiredService<IHashProvider>();
            var identity = CitizenIdentity.Create(new CitizenIdentityId(Guid.NewGuid()), new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()), Guid.NewGuid().ToString(), PhoneNumber.Create(phone));
            identity.VerifyPhoneNumber(PhoneNumber.Create(phone), DateTime.UtcNow);
            db.CitizenIdentities.Add(identity);

            var hashedOtp = hashProvider.Hash(rawOtp);
            var otp = OtpVerification.Create(identity.Id, hashedOtp, PhoneNumber.Create(phone), DateTime.UtcNow.AddMinutes(5));
            db.OtpVerifications.Add(otp);
        });

        var loginRequest = new LoginRequest(phone, rawOtp, "TestDevice");
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        var loginResponseText = await loginResponse.Content.ReadAsStringAsync();
        var json = System.Text.Json.JsonDocument.Parse(loginResponseText);
        var accessToken = json.RootElement.GetProperty("data").GetProperty("accessToken").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Act 1: Disable Citizen
        await SeedDataAsync((db, sp) =>
        {
            var identity = db.CitizenIdentities.First();
            // Simulate disabling by recording failed attempts until locked
            for (int i = 0; i < 6; i++)
            {
                identity.RecordFailedAttempt(5, Guid.NewGuid().ToString(), DateTime.UtcNow);
            }
            db.CitizenIdentities.Update(identity);
        });

        // Act 2: Call protected endpoint
        var protectedCallResponse = await _client.PostAsync("/api/v1/auth/logout", null);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, protectedCallResponse.StatusCode);
    }
}
