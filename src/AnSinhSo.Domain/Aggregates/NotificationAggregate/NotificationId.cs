using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.SeedWork;
using System;

namespace AnSinhSo.Domain.Aggregates.NotificationAggregate;

public sealed class NotificationId : ValueObject
{
    public Guid Value { get; }

    private NotificationId(Guid value)
    {
        Value = value;
    }

    public static NotificationId CreateUnique()
    {
        return new NotificationId(Guid.NewGuid());
    }

    public static NotificationId Create(Guid value)
    {
        return new NotificationId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
