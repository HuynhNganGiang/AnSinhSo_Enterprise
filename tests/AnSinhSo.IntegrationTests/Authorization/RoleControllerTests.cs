using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Commands.CreateRole;
using AnSinhSo.Application.Authorization.Commands.UpdateRole;
using AnSinhSo.Application.Authorization.Commands.AssignRole;
using AnSinhSo.Application.Authorization.Commands.UpdateRolePermissions;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.IntegrationTests.Authentication;
using Xunit;

namespace AnSinhSo.IntegrationTests.Authorization;

public class RoleControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RoleControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetRoles_Unauthorized_Returns401()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/roles");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetRoles_Forbidden_Returns403()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/permission-groups"); // Needs manage permissions, basic user doesn't have it
        request.Headers.Add("Authorization", "Bearer BasicUser");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Role_Create_Success()
    {
        // Arrange
        var request = new CreateRoleRequest("Test Role", "Test Description");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "SystemAdmin");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/roles", request);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new System.Exception($"Request failed with status {response.StatusCode}. Body: {body}");
        }
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Role_Delete_Success()
    {
        // Arrange - Create a role first
        var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/roles");
        createRequest.Headers.Add("Authorization", "Bearer SystemAdmin");
        createRequest.Content = JsonContent.Create(new CreateRoleRequest("Role To Delete", "Test"));
        var createResponse = await _client.SendAsync(createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createResult = await createResponse.Content.ReadFromJsonAsync<AnSinhSo.Shared.Responses.ApiResult<Guid>>();
        var roleId = createResult!.Data;

        // Act - Delete it
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/roles/{roleId}");
        deleteRequest.Headers.Add("Authorization", "Bearer SystemAdmin");
        var deleteResponse = await _client.SendAsync(deleteRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
