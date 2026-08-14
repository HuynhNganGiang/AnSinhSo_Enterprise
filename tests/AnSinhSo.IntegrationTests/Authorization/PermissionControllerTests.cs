using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Constants;
using AnSinhSo.IntegrationTests.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AnSinhSo.IntegrationTests.Authorization;

public class PermissionControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly System.Net.Http.HttpClient _client;

    public PermissionControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task UpdateRolePermissions_ThànhCông_KhiCóQuyềnManage()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "SystemAdmin");

        var roleId = Guid.NewGuid();
        var createRequest = new CreateRoleRequest("Test Role Perms " + roleId, "Test Description");
        var createResponse = await _client.PostAsJsonAsync("/api/v1/roles", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var json = System.Text.Json.JsonDocument.Parse(await createResponse.Content.ReadAsStringAsync());
        var createdRoleId = new Guid(json.RootElement.GetProperty("data").GetString()!);

        // Seed some permissions in DB
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSo.Infrastructure.Persistence.Contexts.AnSinhSoDbContext>();
        var perm1Id = Guid.NewGuid();
        var perm2Id = Guid.NewGuid();
        var groupId = new AnSinhSo.Domain.Aggregates.PermissionGroupAggregate.PermissionGroupId(Guid.NewGuid());
        db.PermissionGroups.Add(AnSinhSo.Domain.Aggregates.PermissionGroupAggregate.PermissionGroup.Create(groupId, "TEST", "Test Group", "Test Description").Value);
        db.Permissions.Add(AnSinhSo.Domain.Aggregates.PermissionAggregate.Permission.Create(new AnSinhSo.Domain.Aggregates.PermissionAggregate.PermissionId(perm1Id), "test.perm", "Test Perm 1", "Description 1", groupId).Value);
        db.Permissions.Add(AnSinhSo.Domain.Aggregates.PermissionAggregate.Permission.Create(new AnSinhSo.Domain.Aggregates.PermissionAggregate.PermissionId(perm2Id), "test.otherperm", "Test Perm 2", "Description 2", groupId).Value);
        await db.SaveChangesAsync();

        // Fetch some permissions to assign
        var permsResponse = await _client.GetAsync("/api/v1/permissions");
        permsResponse.EnsureSuccessStatusCode();
        var permsJson = System.Text.Json.JsonDocument.Parse(await permsResponse.Content.ReadAsStringAsync());
        var permissionsArray = permsJson.RootElement.GetProperty("data").EnumerateArray();
        var permIds = new List<Guid>();
        foreach (var p in permissionsArray)
        {
            if (permIds.Count < 2)
                permIds.Add(p.GetProperty("id").GetGuid());
        }

        var updatePermsRequest = new UpdateRolePermissionsRequest(permIds);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/roles/{createdRoleId}/permissions", updatePermsRequest);

        // Assert
        Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        
        // Act 2: Remove permissions (keep only first one)
        var updatePermsRequest2 = new UpdateRolePermissionsRequest(new List<Guid> { permIds[0] });
        var response2 = await _client.PutAsJsonAsync($"/api/v1/roles/{createdRoleId}/permissions", updatePermsRequest2);
        Assert.True(response2.IsSuccessStatusCode, await response2.Content.ReadAsStringAsync());
    }
}
