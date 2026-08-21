using System;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate;

public sealed class SecurityLog : AggregateRoot<SecurityLogId>
{
    public Guid UserId { get; private set; }
    public SecurityEventType EventType { get; private set; }
    public string Details { get; private set; }
    public string IPAddress { get; private set; }
    

#pragma warning disable CS8618
    private SecurityLog() { }
#pragma warning restore CS8618

    private SecurityLog(SecurityLogId id, Guid userId, SecurityEventType eventType, string details, string ipAddress, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        EventType = eventType;
        Details = details;
        IPAddress = ipAddress;
        CreatedAt = createdAt;
    }

    public static SecurityLog Create(Guid userId, SecurityEventType eventType, string details, string ipAddress)
    {
        return new SecurityLog(SecurityLogId.New(), userId, eventType, details, ipAddress, DateTime.UtcNow);
    }
}
