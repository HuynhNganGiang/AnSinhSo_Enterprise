using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Zalo;

public class ZaloUserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Avatar { get; set; }
}

public interface IZaloOAService
{
    Task<ZaloUserProfileDto?> GetUserProfileAsync(string zaloUserId, CancellationToken cancellationToken = default);
    Task<string> GetAccessTokenAsync(string authorizationCode, CancellationToken cancellationToken = default);
    Task<string> GetValidAccessTokenAsync(CancellationToken cancellationToken = default);
    bool VerifyWebhookSignature(string appId, string timestamp, string mac, string payload);
    Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
}
