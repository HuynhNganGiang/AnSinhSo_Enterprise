using System;

namespace AnSinhSo.Application.Notifications.DTOs;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Guid RecipientCitizenId { get; set; }
    public string? Channel { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public int RetryCount { get; set; }
    public string? SourceModule { get; set; }
    public string? SourceId { get; set; }
}

public class NotificationStatisticsDto
{
    public int Pending { get; set; }
    public int Sent { get; set; }
    public int Failed { get; set; }
    public int Read { get; set; }
    public int Unread { get; set; }
}

public class UnreadCountDto
{
    public int Count { get; set; }
}
