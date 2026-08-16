using System.Collections.Generic;
using AnSinhSo.Application.Notifications.DTOs;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetUnreadNotifications;

public record GetUnreadNotificationsQuery() : IRequest<List<NotificationDto>>;
