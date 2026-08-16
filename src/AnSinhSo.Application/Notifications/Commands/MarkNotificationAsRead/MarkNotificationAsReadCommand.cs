using System;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.MarkNotificationAsRead;

public record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest;
