using System;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.RetryNotification;

public record RetryNotificationCommand(Guid NotificationId) : IRequest;
