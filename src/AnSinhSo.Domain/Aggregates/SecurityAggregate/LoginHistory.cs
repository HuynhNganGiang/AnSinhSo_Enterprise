using System;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.SecurityAggregate;

public sealed class LoginHistory : AggregateRoot<LoginHistoryId>
{
    public Guid UserId { get; private set; }
    public string IPAddress { get; private set; }
    public string DeviceName { get; private set; }
    public string Location { get; private set; }
    public DateTime LoginAt { get; private set; }
    public DateTime? LogoutAt { get; private set; }

#pragma warning disable CS8618
    private LoginHistory() { }
#pragma warning restore CS8618

    private LoginHistory(LoginHistoryId id, Guid userId, string ipAddress, string deviceName, string location, DateTime loginAt)
    {
        Id = id;
        UserId = userId;
        IPAddress = ipAddress;
        DeviceName = deviceName;
        Location = location;
        LoginAt = loginAt;
    }

    public static LoginHistory RecordLogin(Guid userId, string ipAddress, string deviceName, string location)
    {
        return new LoginHistory(LoginHistoryId.New(), userId, ipAddress, deviceName, location, DateTime.UtcNow);
    }

    public void RecordLogout()
    {
        LogoutAt = DateTime.UtcNow;
    }
}
