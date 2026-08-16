using System;
using MediatR;

namespace AnSinhSo.Application.Notifications.Commands.SendNotification;

public record SendNotificationCommand(Guid NotificationId) : IRequest;
