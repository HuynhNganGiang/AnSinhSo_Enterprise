using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.WelfareCases.Commands.CreateWelfareCase;
using AnSinhSo.Application.WelfareCases.DTOs;
using AnSinhSo.Application.WelfarePrograms.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.IntegrationTests.Authentication;
using AnSinhSo.Shared.Responses;
using Xunit;

namespace AnSinhSo.IntegrationTests.Welfare;

public class WelfareCaseIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public WelfareCaseIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task SearchWelfareCases_Unauthorized_Returns401()
    {
        var response = await _client.GetAsync("/api/v1/welfarecases");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SearchWelfareCases_Authorized_Returns200()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/welfarecases?page=1&pageSize=10");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");
        var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed with status {response.StatusCode} and content: {content}");
        }
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult<PagedResult<WelfareCaseDto>>>();
        Assert.NotNull(result);
        Assert.True(result.Success);
    }
}
