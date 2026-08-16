using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.SeedWork;
using System;

namespace AnSinhSo.Domain.Aggregates.NotificationAggregate;

public sealed class NotificationHistory : Entity<NotificationHistoryId>
{
    public NotificationId NotificationId { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string Note { get; private set; }

    private NotificationHistory(
        NotificationHistoryId id,
        NotificationId notificationId,
        NotificationStatus status,
        DateTime timestamp,
        string note) : base(id)
    {
        NotificationId = notificationId;
        Status = status;
        Timestamp = timestamp;
        Note = note;
    }

    public static NotificationHistory Create(
        NotificationId notificationId,
        NotificationStatus status,
        DateTime timestamp,
        string note = "")
    {
        return new NotificationHistory(
            NotificationHistoryId.CreateUnique(),
            notificationId,
            status,
            timestamp,
            note);
    }

#pragma warning disable CS8618
    private NotificationHistory() { }
#pragma warning restore CS8618
}
