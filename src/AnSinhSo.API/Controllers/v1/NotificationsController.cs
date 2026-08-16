using System;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Commands.BroadcastNotification;
using AnSinhSo.Application.Notifications.Commands.CreateNotification;
using AnSinhSo.Application.Notifications.Commands.MarkNotificationAsRead;
using AnSinhSo.Application.Notifications.Commands.RetryNotification;
using AnSinhSo.Application.Notifications.Queries.GetNotificationById;
using AnSinhSo.Application.Notifications.Queries.GetNotifications;
using AnSinhSo.Application.Notifications.Queries.GetNotificationStatistics;
using AnSinhSo.Application.Notifications.Queries.GetUnreadNotifications;
using AnSinhSo.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnSinhSo.API.Controllers;

namespace AnSinhSo.Api.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/notifications")]
public class NotificationsController : ApiControllerBase
{
    private readonly ISender _sender;

    public NotificationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Notifications.View)]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpGet("unread")]
    [Authorize(Policy = Permissions.Notifications.View)]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var result = await _sender.Send(new GetUnreadNotificationsQuery());
        return Ok(result);
    }

    [HttpGet("statistics")]
    [Authorize(Policy = Permissions.Notifications.Statistics)]
    public async Task<IActionResult> GetNotificationStatistics()
    {
        var result = await _sender.Send(new GetNotificationStatisticsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Notifications.View)]
    public async Task<IActionResult> GetNotificationById(Guid id)
    {
        var result = await _sender.Send(new GetNotificationByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Notifications.Create)]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationCommand command)
    {
        var id = await _sender.Send(command);
        return CreatedAtAction(nameof(GetNotificationById), new { id = id }, null);
    }

    [HttpPost("broadcast")]
    [Authorize(Policy = Permissions.Notifications.Broadcast)]
    public async Task<IActionResult> BroadcastNotification([FromBody] BroadcastNotificationCommand command)
    {
        var count = await _sender.Send(command);
        return Ok(new { Count = count });
    }

    [HttpPost("{id}/send")]
    [Authorize(Policy = Permissions.Notifications.Create)]
    public async Task<IActionResult> SendNotification(Guid id)
    {
        await _sender.Send(new AnSinhSo.Application.Notifications.Commands.SendNotification.SendNotificationCommand(id));
        return Ok();
    }

    [HttpPatch("{id}/read")]
    [Authorize(Policy = Permissions.Notifications.Read)]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _sender.Send(new MarkNotificationAsReadCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/retry")]
    [Authorize(Policy = Permissions.Notifications.Retry)]
    public async Task<IActionResult> RetryNotification(Guid id)
    {
        await _sender.Send(new RetryNotificationCommand(id));
        return Ok();
    }
}
