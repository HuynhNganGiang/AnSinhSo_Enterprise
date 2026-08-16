using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;
using AnSinhSo.Domain.SeedWork.Events;
using AnSinhSo.Domain.SeedWork;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;

namespace AnSinhSo.Domain.Events.Notifications;

public record NotificationFailedDomainEvent(NotificationId NotificationId, string Reason, bool IsPermanent) : IDomainEvent;
