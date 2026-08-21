using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.Zalo;

public interface IZaloTokenManager
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    Task<string> GetRefreshTokenAsync(CancellationToken cancellationToken = default);
    Task SaveTokensAsync(string accessToken, string refreshToken, int expiresIn, CancellationToken cancellationToken = default);
}
