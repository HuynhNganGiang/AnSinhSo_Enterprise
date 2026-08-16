using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;
using AnSinhSo.Domain.SeedWork.Events;
using System;
using System.Collections.Generic;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Events.Notifications;
using AnSinhSo.Domain.SeedWork;

namespace AnSinhSo.Domain.Aggregates.NotificationAggregate;

public sealed class Notification : AggregateRoot<NotificationId>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public CitizenId RecipientCitizenId { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public NotificationPriority Priority { get; private set; }
    public NotificationStatus Status { get; private set; }

    public DateTime? ScheduledAt { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    
    public int RetryCount { get; private set; }
    public Guid CorrelationId { get; private set; }
    
    public SourceModule? SourceModule { get; private set; }
    public string? SourceId { get; private set; }

    private readonly List<NotificationHistory> _timeline = new();
    public IReadOnlyCollection<NotificationHistory> Timeline => _timeline.AsReadOnly();

    private Notification(
        NotificationId id,
        string title,
        string content,
        CitizenId recipientCitizenId,
        NotificationChannel channel,
        NotificationPriority priority,
        DateTime? scheduledAt,
        Guid correlationId,
        SourceModule? sourceModule,
        string? sourceId) : base(id)
    {
        Title = title;
        Content = content;
        RecipientCitizenId = recipientCitizenId;
        Channel = channel;
        Priority = priority;
        ScheduledAt = scheduledAt;
        CorrelationId = correlationId;
        SourceModule = sourceModule;
        SourceId = sourceId;

        Status = NotificationStatus.Pending;
        RetryCount = 0;
    }

    public static Notification Create(
        string title,
        string content,
        CitizenId recipientCitizenId,
        NotificationChannel channel,
        NotificationPriority priority,
        DateTime? scheduledAt = null,
        Guid? correlationId = null,
        SourceModule? sourceModule = null,
        string? sourceId = null)
    {
        var notification = new Notification(
            NotificationId.CreateUnique(),
            title,
            content,
            recipientCitizenId,
            channel,
            priority,
            scheduledAt,
            correlationId ?? Guid.NewGuid(),
            sourceModule,
            sourceId);

        notification.AddTimelineEntry(NotificationStatus.Pending, "Notification Created");
        notification.AddDomainEvent(new NotificationCreatedDomainEvent(notification.Id));

        return notification;
    }

    public void MarkAsProcessing()
    {
        if (Status == NotificationStatus.Sent || Status == NotificationStatus.Read)
            return;

        Status = NotificationStatus.Processing;
        AddTimelineEntry(NotificationStatus.Processing, "Processing notification");
    }

    public void MarkAsSent(DateTime sentAt)
    {
        Status = NotificationStatus.Sent;
        SentAt = sentAt;
        AddTimelineEntry(NotificationStatus.Sent, "Notification Sent Successfully");
        AddDomainEvent(new NotificationSentDomainEvent(Id));
    }

    public void MarkAsFailed(string reason)
    {
        RetryCount++;
        
        if (RetryCount >= 3)
        {
            Status = NotificationStatus.Failed;
            AddTimelineEntry(NotificationStatus.Failed, $"Failed permanently after {RetryCount} retries. Reason: {reason}");
        }
        else
        {
            Status = NotificationStatus.Pending; // go back to pending for retry
            AddTimelineEntry(NotificationStatus.Pending, $"Failed. Retry {RetryCount}/3. Reason: {reason}");
        }
        
        AddDomainEvent(new NotificationFailedDomainEvent(Id, reason, RetryCount >= 3));
    }

    public void MarkAsRead(DateTime readAt)
    {
        if (Status != NotificationStatus.Sent)
            return;

        Status = NotificationStatus.Read;
        ReadAt = readAt;
        AddTimelineEntry(NotificationStatus.Read, "Notification Read");
        AddDomainEvent(new NotificationReadDomainEvent(Id));
    }

    private void AddTimelineEntry(NotificationStatus status, string note)
    {
        _timeline.Add(NotificationHistory.Create(Id, status, DateTime.UtcNow, note));
    }

#pragma warning disable CS8618
    private Notification() { }
#pragma warning restore CS8618
}
