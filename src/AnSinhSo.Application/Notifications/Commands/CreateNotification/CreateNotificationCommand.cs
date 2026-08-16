using System;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.CreateNotification;

public record CreateNotificationCommand(
    string Title,
    string Content,
    Guid RecipientCitizenId,
    NotificationChannel Channel,
    NotificationPriority Priority,
    DateTime? ScheduledAt,
    SourceModule? SourceModule,
    string? SourceId) : IRequest<Guid>;
