using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Zalo;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Services.Zalo;

public class ZaloTokenManager : IZaloTokenManager
{
    private readonly IDistributedCache _cache;
    private readonly ZaloOptions _options;
    
    private const string AccessTokenKey = "ZaloOA:AccessToken";
    private const string RefreshTokenKey = "ZaloOA:RefreshToken";

    public ZaloTokenManager(IDistributedCache cache, IOptions<ZaloOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = await _cache.GetStringAsync(AccessTokenKey, cancellationToken);
        if (!string.IsNullOrEmpty(token))
        {
            return token;
        }

        // Fallback to options if not in cache (initial startup)
        return _options.AccessToken;
    }

    public async Task<string> GetRefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = await _cache.GetStringAsync(RefreshTokenKey, cancellationToken);
        if (!string.IsNullOrEmpty(token))
        {
            return token;
        }

        // Fallback to options
        return _options.RefreshToken;
    }

    public async Task SaveTokensAsync(string accessToken, string refreshToken, int expiresIn, CancellationToken cancellationToken = default)
    {
        var accessTokenOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiresIn - 300) // Buffer 5 minutes
        };
        
        await _cache.SetStringAsync(AccessTokenKey, accessToken, accessTokenOptions, cancellationToken);
        await _cache.SetStringAsync(RefreshTokenKey, refreshToken, new DistributedCacheEntryOptions(), cancellationToken);
    }
}
