using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.SeedWork;
using System;

namespace AnSinhSo.Domain.Aggregates.NotificationAggregate;

public sealed class NotificationHistoryId : ValueObject
{
    public Guid Value { get; }

    private NotificationHistoryId(Guid value)
    {
        Value = value;
    }

    public static NotificationHistoryId CreateUnique()
    {
        return new NotificationHistoryId(Guid.NewGuid());
    }

    public static NotificationHistoryId Create(Guid value)
    {
        return new NotificationHistoryId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
