using System;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate;

public sealed class AuditLogin : AggregateRoot<AuditLoginId>
{
    public Guid UserId { get; private set; }
    public bool IsSuccess { get; private set; }
    public string IPAddress { get; private set; }
    public string UserAgent { get; private set; }
    public string TimeZone { get; private set; }
    public string FailureReason { get; private set; }
    

#pragma warning disable CS8618
    private AuditLogin() { }
#pragma warning restore CS8618

    private AuditLogin(AuditLoginId id, Guid userId, bool isSuccess, string ipAddress, string userAgent, string timeZone, string failureReason, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        IsSuccess = isSuccess;
        IPAddress = ipAddress;
        UserAgent = userAgent;
        TimeZone = timeZone;
        FailureReason = failureReason;
        CreatedAt = createdAt;
    }

    public static AuditLogin CreateSuccess(Guid userId, string ipAddress, string userAgent, string timeZone)
    {
        return new AuditLogin(AuditLoginId.New(), userId, true, ipAddress, userAgent, timeZone, string.Empty, DateTime.UtcNow);
    }

    public static AuditLogin CreateFailure(Guid userId, string ipAddress, string userAgent, string timeZone, string failureReason)
    {
        return new AuditLogin(AuditLoginId.New(), userId, false, ipAddress, userAgent, timeZone, failureReason, DateTime.UtcNow);
    }
}
