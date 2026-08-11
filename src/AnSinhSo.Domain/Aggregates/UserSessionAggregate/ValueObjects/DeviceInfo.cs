using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;

public sealed class DeviceInfo : ValueObject
{
    public string IpAddress { get; }
    public string UserAgent { get; }
    public string DeviceName { get; }

    private DeviceInfo(string ipAddress, string userAgent, string deviceName)
    {
        IpAddress = ipAddress;
        UserAgent = userAgent;
        DeviceName = deviceName;
    }

    public static DeviceInfo Create(string ipAddress, string userAgent, string deviceName)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("IP Address cannot be empty.", nameof(ipAddress));

        if (string.IsNullOrWhiteSpace(userAgent))
            throw new ArgumentException("User Agent cannot be empty.", nameof(userAgent));

        return new DeviceInfo(ipAddress, userAgent, string.IsNullOrWhiteSpace(deviceName) ? "Unknown Device" : deviceName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IpAddress;
        yield return UserAgent;
        yield return DeviceName;
    }
}
