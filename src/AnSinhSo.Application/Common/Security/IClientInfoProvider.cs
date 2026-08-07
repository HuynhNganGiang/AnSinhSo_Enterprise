namespace AnSinhSo.Application.Common.Security;

public interface IClientInfoProvider
{
    string IpAddress { get; }
    string DeviceName { get; }
    string UserAgent { get; }
}
