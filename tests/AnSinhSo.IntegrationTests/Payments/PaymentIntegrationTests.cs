using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Payments.Commands.CreatePayment;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.IntegrationTests.Authentication;
using AnSinhSo.Shared.Responses;
using Xunit;

namespace AnSinhSo.IntegrationTests.Payments;

public class PaymentIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public PaymentIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task SearchPayments_Unauthorized_Returns401()
    {
        var response = await _client.GetAsync("/api/v1/payments");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SearchPayments_Authorized_Returns200()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/payments?page=1&pageSize=10");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult<PagedResult<PaymentDto>>>();
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetPaymentById_Unauthorized_Returns401()
    {
        var response = await _client.GetAsync($"/api/v1/payments/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPaymentById_NotFound_Returns404()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/payments/{Guid.NewGuid()}");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCitizenPayments_Authorized_Returns200()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/payments/citizen/{Guid.NewGuid()}");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult<List<PaymentDto>>>();
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetHouseholdPayments_Authorized_Returns200()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/payments/household/{Guid.NewGuid()}");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult<List<PaymentDto>>>();
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetPendingPayments_Authorized_Returns200()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/payments/pending");
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResult<List<PaymentDto>>>();
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task CreatePayment_Unauthorized_Returns401()
    {
        var command = new CreatePaymentCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1000, DateTime.UtcNow, 1, "Test Note");
        var response = await _client.PostAsJsonAsync("/api/v1/payments", command);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreatePayment_ValidationFailed_AmountZero_Returns400()
    {
        var command = new CreatePaymentCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, DateTime.UtcNow, 1, "Test Note");
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/payments")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Add("Authorization", "Bearer SystemAdmin");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ApprovePayment_Unauthorized_Returns401()
    {
        var response = await _client.PutAsync($"/api/v1/payments/{Guid.NewGuid()}/approve", null);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CompletePayment_Unauthorized_Returns401()
    {
        var response = await _client.PutAsJsonAsync($"/api/v1/payments/{Guid.NewGuid()}/complete", DateTime.UtcNow);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task FailPayment_Unauthorized_Returns401()
    {
        var response = await _client.PutAsJsonAsync($"/api/v1/payments/{Guid.NewGuid()}/fail", "Reason");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CancelPayment_Unauthorized_Returns401()
    {
        var response = await _client.PutAsJsonAsync($"/api/v1/payments/{Guid.NewGuid()}/cancel", "Reason");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
