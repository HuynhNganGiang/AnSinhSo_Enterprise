using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AnSinhSo.Infrastructure.Services.Zalo;
using AnSinhSo.IntegrationTests.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace AnSinhSo.IntegrationTests.Zalo;

public class ZaloControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ZaloControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Webhook_ShouldReturnOk_ForValidPayload()
    {
        // Arrange
        var options = _factory.Services.GetRequiredService<IOptions<ZaloOptions>>().Value;
        
        var payload = new
        {
            event_name = "follow",
            app_id = options.AppId,
            sender = new
            {
                id = "zalo_user_id"
            },
            recipient = new
            {
                id = "oa_id"
            },
            timestamp = "1501234567890"
        };
        
        var rawPayload = JsonSerializer.Serialize(payload);
        var timestamp = "1501234567890";
        var dataToSign = $"{options.AppId}{rawPayload}{timestamp}{options.AppSecret}";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
        var mac = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        var content = new StringContent(rawPayload, Encoding.UTF8, "application/json");
        content.Headers.Add("X-ZEngine-Mac", mac);
        content.Headers.Add("X-ZEngine-Timestamp", timestamp);

        // Act
        var response = await _client.PostAsync("/api/v1/zalo/webhook", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
