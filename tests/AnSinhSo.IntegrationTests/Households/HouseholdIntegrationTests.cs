using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Households.Commands.CreateHousehold;
using AnSinhSo.Application.Households.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.IntegrationTests.Authentication;
using AnSinhSo.Shared.Responses;
using Xunit;

namespace AnSinhSo.IntegrationTests.Households;

public class HouseholdIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public HouseholdIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task SearchHouseholds_Unauthorized_Returns401()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/households");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SearchHouseholds_Authorized_Returns200()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/households?page=1&pageSize=10");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult<PagedResult<HouseholdSummaryDto>>>();
        Assert.NotNull(result);
        Assert.True(result.Success);
    }
}
