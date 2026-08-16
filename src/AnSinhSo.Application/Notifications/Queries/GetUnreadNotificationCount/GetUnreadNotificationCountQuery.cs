using System;
using AnSinhSo.Application.Notifications.DTOs;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetUnreadNotificationCount;

public record GetUnreadNotificationCountQuery(Guid CitizenId) : IRequest<UnreadCountDto>;
