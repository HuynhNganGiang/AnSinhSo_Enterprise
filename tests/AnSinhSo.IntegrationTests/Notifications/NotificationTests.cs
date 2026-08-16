using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Commands.CreateNotification;
using AnSinhSo.Application.Notifications.Commands.MarkNotificationAsRead;
using AnSinhSo.Application.Notifications.DTOs;
using FluentAssertions;
using Xunit;
using AnSinhSo.Shared.Responses;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using AnSinhSo.IntegrationTests.Authentication;

namespace AnSinhSo.IntegrationTests.Notifications;

public class NotificationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public NotificationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Notification_Timeline_And_Retry_Should_Work_Correctly()
    {
        // 1. Authorize User
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "SystemAdmin");

        // 2. Create Notification
        var createCommand = new CreateNotificationCommand(
            "Test Notification",
            "This is a test notification.",
            Guid.NewGuid(),
            NotificationChannel.InApp,
            NotificationPriority.Normal,
            null,
            null,
            null
        );

        var createResponse = await _client.PostAsJsonAsync("/api/v1/notifications", createCommand);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // Wait, how do I get the ID? From Location header
        var location = createResponse.Headers.Location;
        var getResponse = await _client.GetAsync(location);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await getResponse.Content.ReadFromJsonAsync<ApiResult<NotificationDto>>();
        var notification = result!.Data;
        
        notification!.Status.Should().Be("Pending");
        notification!.Status.Should().Be(NotificationStatus.Pending.ToString());

        // 3. Send Notification
        var sendResponse = await _client.PostAsync($"/api/v1/notifications/{notification.Id}/send", null);
        sendResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify Status = Sent
        var getResponse2 = await _client.GetAsync(location);
        var result2 = await getResponse2.Content.ReadFromJsonAsync<ApiResult<NotificationDto>>();
        var notification2 = result2!.Data;
        notification2!.Status.Should().Be(NotificationStatus.Sent.ToString());

        // 4. Mark As Read
        var readResponse = await _client.PatchAsync($"/api/v1/notifications/{notification.Id}/read", null);
        readResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify Status = Read
        var getResponse3 = await _client.GetAsync(location);
        var result3 = await getResponse3.Content.ReadFromJsonAsync<ApiResult<NotificationDto>>();
        var notification3 = result3!.Data;
        notification3!.Status.Should().Be(NotificationStatus.Read.ToString());
    }
}
