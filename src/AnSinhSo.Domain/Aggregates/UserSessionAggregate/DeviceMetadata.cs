using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.UserSessionAggregate;

public class DeviceMetadata : ValueObject
{
    public string DeviceName { get; private set; }
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }

#pragma warning disable CS8618
    private DeviceMetadata() { } // ORM
#pragma warning restore CS8618

    public DeviceMetadata(string deviceName, string ipAddress, string userAgent)
    {
        DeviceName = deviceName;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DeviceName;
        yield return IpAddress;
        yield return UserAgent;
    }
}
