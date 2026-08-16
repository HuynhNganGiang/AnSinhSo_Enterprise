using AnSinhSo.Application.Notifications.DTOs;
using MediatR;

namespace AnSinhSo.Application.Notifications.Queries.GetNotificationStatistics;

public record GetNotificationStatisticsQuery() : IRequest<NotificationStatisticsDto>;
