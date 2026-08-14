using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.IntegrationTests.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AnSinhSo.IntegrationTests.Authorization;

public class UserRolesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly System.Net.Http.HttpClient _client;

    public UserRolesControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task AssignRole_ThànhCông_KhiCóQuyềnManage()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "SystemAdmin");

        // Create role first to ensure it exists
        var createRequest = new CreateRoleRequest("Test Role " + Guid.NewGuid(), "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/v1/roles", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var json = System.Text.Json.JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync());
        var createdRoleId = new Guid(json.RootElement.GetProperty("data").GetString()!);

        // Seed a CitizenIdentity
        var userId = Guid.NewGuid();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSo.Infrastructure.Persistence.Contexts.AnSinhSoDbContext>();
        var identity = AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentity.Create(
            new AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId(userId),
            new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()),
            Guid.NewGuid().ToString(),
            AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects.PhoneNumber.Create("0900000001")
        );
        db.CitizenIdentities.Add(identity);
        await db.SaveChangesAsync();

        var assignRequest = new AssignRoleRequest(createdRoleId);

        // Act
        var response = await _client.PostAsJsonAsync($"/api/v1/users/{userId}/roles", assignRequest);

        // Assert
        Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task GetUserRoles_ReturnsAssignedRoles()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "SystemAdmin");

        var roleId = Guid.NewGuid();
        var createRequest = new CreateRoleRequest("Test Role Get " + roleId, "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/v1/roles", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var json2 = System.Text.Json.JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync());
        var createdRoleId2 = new Guid(json2.RootElement.GetProperty("data").GetString()!);

        // Seed a CitizenIdentity
        var userId = Guid.NewGuid();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSo.Infrastructure.Persistence.Contexts.AnSinhSoDbContext>();
        var identity = AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentity.Create(
            new AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId(userId),
            new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()),
            Guid.NewGuid().ToString(),
            AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects.PhoneNumber.Create("0900000002")
        );
        db.CitizenIdentities.Add(identity);
        await db.SaveChangesAsync();

        var assignRequest = new AssignRoleRequest(createdRoleId2);
        await _client.PostAsJsonAsync($"/api/v1/users/{userId}/roles", assignRequest);

        // Act
        var getResponse = await _client.GetAsync($"/api/v1/users/{userId}/roles");
        
        // Assert
        getResponse.EnsureSuccessStatusCode();
        var getContent = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains(createdRoleId2.ToString(), getContent);
    }
}
