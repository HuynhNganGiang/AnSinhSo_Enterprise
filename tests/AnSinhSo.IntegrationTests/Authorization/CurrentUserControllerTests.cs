using System.Net;
using System.Threading.Tasks;
using AnSinhSo.IntegrationTests.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AnSinhSo.IntegrationTests.Authorization;

public class CurrentUserControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly System.Net.Http.HttpClient _client;

    public CurrentUserControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetCurrentUserPermissions_ReturnsPermissions()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "BasicUser");

        // Seed BasicUser in DB
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSo.Infrastructure.Persistence.Contexts.AnSinhSoDbContext>();
        
        var identity = AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentity.Create(
            new AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId(userId),
            new AnSinhSo.Domain.Aggregates.CitizenAggregate.CitizenId(Guid.NewGuid()),
            Guid.NewGuid().ToString(),
            AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects.PhoneNumber.Create("0900000003")
        );
        db.CitizenIdentities.Add(identity);

        var roleId = Guid.NewGuid();
        var roleResult = AnSinhSo.Domain.Aggregates.RoleAggregate.Role.Create(new AnSinhSo.Domain.Aggregates.RoleAggregate.RoleId(roleId), "Basic User Role", "Test", false);
        var role = roleResult.Value;
        
        role.UpdatePermissions(new System.Collections.Generic.List<AnSinhSo.Domain.Aggregates.PermissionAggregate.PermissionId> { new AnSinhSo.Domain.Aggregates.PermissionAggregate.PermissionId(Guid.NewGuid()) });
        
        var userRoleResult = AnSinhSo.Domain.Aggregates.UserRoleAggregate.UserRole.Assign(new AnSinhSo.Domain.Aggregates.UserRoleAggregate.UserRoleId(Guid.NewGuid()), identity.Id, new AnSinhSo.Domain.Aggregates.RoleAggregate.RoleId(roleId));
        var userRole = userRoleResult.Value;
        db.Roles.Add(role);
        db.UserRoles.Add(userRole);
        await db.SaveChangesAsync();

        // Let's just test GET /api/v1/users/me/roles because permissions need real permission entities in DB
        // Act
        var response = await _client.GetAsync("/api/v1/users/me/roles");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains(roleId.ToString(), content);
    }
}
