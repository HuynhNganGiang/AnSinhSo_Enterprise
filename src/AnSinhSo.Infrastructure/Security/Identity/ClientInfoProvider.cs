using AnSinhSo.Application.Common.Security;
using Microsoft.AspNetCore.Http;

namespace AnSinhSo.Infrastructure.Security.Identity;

public class ClientInfoProvider : IClientInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string IpAddress => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";
    public string DeviceName => "Unknown Device";
    public string UserAgent => _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown Browser";
}
