using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Zalo;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Services.Zalo;

public class ZaloOAService : IZaloOAService
{
    private readonly HttpClient _httpClient;
    private readonly ZaloOptions _options;
    private readonly IZaloTokenManager _tokenManager;

    public ZaloOAService(HttpClient httpClient, IOptions<ZaloOptions> options, IZaloTokenManager tokenManager)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _tokenManager = tokenManager;
        _httpClient.BaseAddress = new Uri("https://openapi.zalo.me/");
    }

    public async Task<string> GetAccessTokenAsync(string authorizationCode, CancellationToken cancellationToken = default)
    {
        // The real API is https://oauth.zaloapp.com/v4/oa/access_token
        // with grant_type=authorization_code, code=..., app_id=..., secret_key=...
        
        using var client = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("app_id", _options.AppId),
        });
        
        client.DefaultRequestHeaders.Add("secret_key", _options.AppSecret);
        var response = await client.PostAsync("https://oauth.zaloapp.com/v4/oa/access_token", content, cancellationToken);
        
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(responseJson);
        
        var accessToken = doc.RootElement.GetProperty("access_token").GetString();
        var refreshToken = doc.RootElement.GetProperty("refresh_token").GetString();
        var expiresIn = int.Parse(doc.RootElement.GetProperty("expires_in").GetString()!);
        
        await _tokenManager.SaveTokensAsync(accessToken!, refreshToken!, expiresIn, cancellationToken);
        return accessToken!;
    }

    public async Task<string> GetValidAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var accessToken = await _tokenManager.GetAccessTokenAsync(cancellationToken);
        if (!string.IsNullOrEmpty(accessToken) && accessToken != _options.AccessToken)
        {
            return accessToken; // valid cached token
        }
        
        // Try refresh
        var refreshToken = await _tokenManager.GetRefreshTokenAsync(cancellationToken);
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new InvalidOperationException("No refresh token available");
        }
        
        using var client = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
            new KeyValuePair<string, string>("app_id", _options.AppId),
        });
        
        client.DefaultRequestHeaders.Add("secret_key", _options.AppSecret);
        var response = await client.PostAsync("https://oauth.zaloapp.com/v4/oa/access_token", content, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            // If failed to refresh, we return the old option access token as last resort
            return _options.AccessToken;
        }
        
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(responseJson);
        
        if (doc.RootElement.TryGetProperty("error", out _))
        {
            return _options.AccessToken;
        }
        
        var newAccessToken = doc.RootElement.GetProperty("access_token").GetString();
        var newRefreshToken = doc.RootElement.GetProperty("refresh_token").GetString();
        var expiresIn = int.Parse(doc.RootElement.GetProperty("expires_in").GetString()!);
        
        await _tokenManager.SaveTokensAsync(newAccessToken!, newRefreshToken!, expiresIn, cancellationToken);
        return newAccessToken!;
    }

    public async Task<ZaloUserProfileDto?> GetUserProfileAsync(string zaloUserId, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetValidAccessTokenAsync(cancellationToken);
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"v2.0/oa/getprofile?data={{\"user_id\":\"{zaloUserId}\"}}");
        request.Headers.Add("access_token", accessToken);
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(responseJson);
        
        if (doc.RootElement.TryGetProperty("error", out var errorElement) && errorElement.GetInt32() != 0)
        {
            return null;
        }
        
        var data = doc.RootElement.GetProperty("data");
        
        return new ZaloUserProfileDto
        {
            Id = zaloUserId,
            Name = data.TryGetProperty("display_name", out var name) ? name.GetString() ?? "" : "",
            Avatar = data.TryGetProperty("avatar", out var avatar) ? avatar.GetString() : null
        };
    }
    
    public bool VerifyWebhookSignature(string appId, string timestamp, string mac, string payload)
    {
        if (appId != _options.AppId)
        {
            return false;
        }
        
        var dataToSign = $"{_options.AppId}{payload}{timestamp}{_options.AppSecret}";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
        var computedMac = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        
        return string.Equals(mac, computedMac, StringComparison.OrdinalIgnoreCase);
    }
    
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var accessToken = await GetValidAccessTokenAsync(cancellationToken);
            var request = new HttpRequestMessage(HttpMethod.Get, "v2.0/oa/getoa");
            request.Headers.Add("access_token", accessToken);
            
            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
