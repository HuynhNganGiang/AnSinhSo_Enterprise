using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Authentication.BackOffice.BackOfficeLogin;
using AnSinhSo.Application.Authentication.BackOffice.RefreshToken;
using AnSinhSo.Application.Common.Security;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Net.Http.Headers;
using AnSinhSo.Contracts.Authentication;

namespace AnSinhSo.IntegrationTests.Authentication;

public class BackOfficeAuthenticationTests : IClassFixture<RealAuthWebApplicationFactory>
{
    private readonly RealAuthWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BackOfficeAuthenticationTests(RealAuthWebApplicationFactory factory)
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
    public async Task Test1_ChangePassword_RejectOldJwt()
    {
        var username = "admin_test1";
        var password = "StrongPassword123!";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var passwordHasher = sp.GetRequiredService<IPasswordHasher>();
            var hash = passwordHasher.Hash(password);
            var user = User.Create(username, "admin1@test.com", hash);
            db.Users.Add(user);
        });

        // 1. Login to get JWT
        var request = new BackOfficeLoginRequest(username, password, "Device1");
        var response = await _client.PostAsJsonAsync("/api/v1/auth/backoffice/login", request);
        Assert.True(response.IsSuccessStatusCode);
        
        var json = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var accessToken = json.GetProperty("data").GetProperty("accessToken").GetString();

        // 2. Use JWT to access protected route -> SHOULD PASS
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var protectedResponse1 = await _client.GetAsync("/api/v1/dev/protected");
        
        // Note: Replace with actual protected endpoint if /dev/protected doesn't exist
        // The dev controller has [Authorize], so any authorize endpoint will work
        // For simplicity, we can just call logout, which requires Auth
        var checkResponse1 = await _client.PostAsync("/api/v1/auth/logout", null);
        Assert.NotEqual(HttpStatusCode.Unauthorized, checkResponse1.StatusCode);

        // 3. Change password in DB
        await SeedDataAsync((db, sp) =>
        {
            var user = db.Users.First(u => u.Username == username);
            user.ChangePasswordHash("NewHashDoesNotMatter");
            db.Users.Update(user);
        });

        // 4. Use old JWT to access protected route -> SHOULD FAIL (401)
        var checkResponse2 = await _client.PostAsync("/api/v1/auth/logout", null);
        Assert.Equal(HttpStatusCode.Unauthorized, checkResponse2.StatusCode);
    }

    [Fact]
    public async Task Test2_AdminLockAccount_RejectOldJwt()
    {
        var username = "admin_test2";
        var password = "StrongPassword123!";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var passwordHasher = sp.GetRequiredService<IPasswordHasher>();
            var user = User.Create(username, "admin2@test.com", passwordHasher.Hash(password));
            db.Users.Add(user);
        });

        // 1. Login
        var request = new BackOfficeLoginRequest(username, password, "Device1");
        var response = await _client.PostAsJsonAsync("/api/v1/auth/backoffice/login", request);
        Assert.True(response.IsSuccessStatusCode);
        
        var json = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var accessToken = json.GetProperty("data").GetProperty("accessToken").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // 2. Lock account
        await SeedDataAsync((db, sp) =>
        {
            var user = db.Users.First(u => u.Username == username);
            user.RecordAccessFailed(1, TimeSpan.FromDays(1)); // Lock immediately
            db.Users.Update(user);
        });

        // 3. Use old JWT -> SHOULD FAIL
        var checkResponse = await _client.PostAsync("/api/v1/auth/logout", null);
        Assert.Equal(HttpStatusCode.Unauthorized, checkResponse.StatusCode);
    }

    [Fact]
    public async Task Test3_LogoutAllDevices_RejectRefreshToken_And_Jwt()
    {
        var username = "admin_test3";
        var password = "StrongPassword123!";
        
        await SeedDataAsync((db, sp) =>
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var passwordHasher = sp.GetRequiredService<IPasswordHasher>();
            var user = User.Create(username, "admin3@test.com", passwordHasher.Hash(password));
            db.Users.Add(user);
        });

        // 1. Login
        var request = new BackOfficeLoginRequest(username, password, "Device1");
        var response = await _client.PostAsJsonAsync("/api/v1/auth/backoffice/login", request);
        
        var json = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var accessToken = json.GetProperty("data").GetProperty("accessToken").GetString();
        var refreshToken = json.GetProperty("data").GetProperty("refreshToken").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // 2. Logout all devices
        var logoutAllResponse = await _client.PostAsync("/api/v1/auth/logout-all-sessions", null);
        Assert.NotEqual(HttpStatusCode.Unauthorized, logoutAllResponse.StatusCode);

        // 3. Use JWT -> SHOULD FAIL
        var checkResponse = await _client.PostAsync("/api/v1/auth/logout", null);
        Assert.Equal(HttpStatusCode.Unauthorized, checkResponse.StatusCode);

        // 4. Use Refresh Token -> SHOULD FAIL
        var refreshRequest = new RefreshTokenRequest(refreshToken!, "TestDevice");
        var refreshResponse = await _client.PostAsJsonAsync("/api/v1/auth/refresh-token", refreshRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, refreshResponse.StatusCode);
    }
}
