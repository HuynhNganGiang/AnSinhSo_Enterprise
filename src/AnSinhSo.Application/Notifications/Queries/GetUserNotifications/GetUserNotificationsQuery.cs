using System;
using System.Collections.Generic;
using AnSinhSo.Application.Notifications.DTOs;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetUserNotifications;

public record GetUserNotificationsQuery(Guid CitizenId) : IRequest<List<NotificationDto>>;
