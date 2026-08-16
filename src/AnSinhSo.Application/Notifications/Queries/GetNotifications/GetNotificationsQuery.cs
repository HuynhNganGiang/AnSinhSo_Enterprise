using System;
using System.Collections.Generic;
using AnSinhSo.Application.Notifications.DTOs;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(
    string? Keyword = null,
    DateTime? Date = null,
    NotificationChannel? Channel = null,
    NotificationStatus? Status = null,
    NotificationPriority? Priority = null,
    SourceModule? SourceModule = null) : IRequest<List<NotificationDto>>;
