using System;
using System.Collections.Generic;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.BroadcastNotification;

public record BroadcastNotificationCommand(
    string Title,
    string Content,
    NotificationChannel Channel,
    NotificationPriority Priority,
    RecipientFilter Filter,
    List<Guid> SpecificCitizenIds,
    SourceModule? SourceModule,
    string SourceId) : IRequest<int>;
