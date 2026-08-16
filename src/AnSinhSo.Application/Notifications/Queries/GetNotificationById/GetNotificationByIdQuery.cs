using System;
using AnSinhSo.Application.Notifications.DTOs;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetNotificationById;

public record GetNotificationByIdQuery(Guid Id) : IRequest<NotificationDto>;
