using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Authentication.RateLimiting;
using AnSinhSo.Application.Authentication.Citizen;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Authentication;

public class OtpRateLimitService : IOtpRateLimitService
{
    private readonly IDistributedCache _cache;
    private readonly IRateLimitKeyBuilder _keyBuilder;
    private readonly OtpOptions _options;

    public OtpRateLimitService(IDistributedCache cache, IRateLimitKeyBuilder keyBuilder, IOptions<OtpOptions> options)
    {
        _cache = cache;
        _keyBuilder = keyBuilder;
        _options = options.Value;
    }

    public async Task<RateLimitResult> CheckRateLimitAsync(string phoneNumber, string ip, string deviceFingerprint, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // 1. Check Cooldown
        var cooldownKey = _keyBuilder.BuildKey(RateLimitScope.Phone, phoneNumber, "Cooldown");
        var inCooldown = await _cache.GetStringAsync(cooldownKey, cancellationToken);
        if (!string.IsNullOrEmpty(inCooldown))
        {
            return RateLimitResult.Blocked(RateLimitType.Cooldown, RateLimitScope.Phone, "Cooldown active. Please wait.", _options.CooldownSeconds, 1, now + _options.CooldownSeconds);
        }

        // 2. Check Hourly Limits
        var phoneHourKey = _keyBuilder.BuildKey(RateLimitScope.Phone, phoneNumber, "Hourly");
        if (await IsLimitExceeded(phoneHourKey, _options.MaxRequestsPerHour, cancellationToken))
        {
            return RateLimitResult.Blocked(RateLimitType.HourlyLimit, RateLimitScope.Phone, "Hourly limit exceeded for phone.", 3600, _options.MaxRequestsPerHour, now + 3600);
        }

        var ipHourKey = _keyBuilder.BuildKey(RateLimitScope.IP, ip, "Hourly");
        if (await IsLimitExceeded(ipHourKey, _options.MaxRequestsPerHour, cancellationToken)) // using same limit for simplicity or create MaxIpRequestsPerHour
        {
            return RateLimitResult.Blocked(RateLimitType.HourlyLimit, RateLimitScope.IP, "Hourly limit exceeded for IP.", 3600, _options.MaxRequestsPerHour, now + 3600);
        }

        var deviceHourKey = _keyBuilder.BuildKey(RateLimitScope.Device, deviceFingerprint, "Hourly");
        if (await IsLimitExceeded(deviceHourKey, _options.MaxDeviceRequestsPerHour, cancellationToken))
        {
            return RateLimitResult.Blocked(RateLimitType.HourlyLimit, RateLimitScope.Device, "Hourly limit exceeded for Device.", 3600, _options.MaxDeviceRequestsPerHour, now + 3600);
        }

        // 3. Check Daily Limits
        var phoneDayKey = _keyBuilder.BuildKey(RateLimitScope.Phone, phoneNumber, "Daily");
        if (await IsLimitExceeded(phoneDayKey, _options.MaxRequestsPerDay, cancellationToken))
        {
            return RateLimitResult.Blocked(RateLimitType.DailyLimit, RateLimitScope.Phone, "Daily limit exceeded for phone.", 86400, _options.MaxRequestsPerDay, now + 86400);
        }

        var ipDayKey = _keyBuilder.BuildKey(RateLimitScope.IP, ip, "Daily");
        if (await IsLimitExceeded(ipDayKey, _options.MaxRequestsPerDay, cancellationToken))
        {
            return RateLimitResult.Blocked(RateLimitType.DailyLimit, RateLimitScope.IP, "Daily limit exceeded for IP.", 86400, _options.MaxRequestsPerDay, now + 86400);
        }

        var deviceDayKey = _keyBuilder.BuildKey(RateLimitScope.Device, deviceFingerprint, "Daily");
        if (await IsLimitExceeded(deviceDayKey, _options.MaxDeviceRequestsPerDay, cancellationToken))
        {
            return RateLimitResult.Blocked(RateLimitType.DailyLimit, RateLimitScope.Device, "Daily limit exceeded for Device.", 86400, _options.MaxDeviceRequestsPerDay, now + 86400);
        }

        return RateLimitResult.Success(_options.MaxRequestsPerHour, _options.MaxRequestsPerHour - await GetCount(phoneHourKey, cancellationToken), now + 3600);
    }

    public async Task RecordSuccessAsync(string phoneNumber, string ip, string deviceFingerprint, CancellationToken cancellationToken = default)
    {
        // Set cooldown
        var cooldownKey = _keyBuilder.BuildKey(RateLimitScope.Phone, phoneNumber, "Cooldown");
        await _cache.SetStringAsync(cooldownKey, "1", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.CooldownSeconds) }, cancellationToken);

        // Increment Phone Limits
        var phoneHourKey = _keyBuilder.BuildKey(RateLimitScope.Phone, phoneNumber, "Hourly");
        await IncrementCounter(phoneHourKey, TimeSpan.FromHours(1), cancellationToken);
        var phoneDayKey = _keyBuilder.BuildKey(RateLimitScope.Phone, phoneNumber, "Daily");
        await IncrementCounter(phoneDayKey, TimeSpan.FromDays(1), cancellationToken);

        // Increment IP Limits
        var ipHourKey = _keyBuilder.BuildKey(RateLimitScope.IP, ip, "Hourly");
        await IncrementCounter(ipHourKey, TimeSpan.FromHours(1), cancellationToken);
        var ipDayKey = _keyBuilder.BuildKey(RateLimitScope.IP, ip, "Daily");
        await IncrementCounter(ipDayKey, TimeSpan.FromDays(1), cancellationToken);

        // Increment Device Limits
        var deviceHourKey = _keyBuilder.BuildKey(RateLimitScope.Device, deviceFingerprint, "Hourly");
        await IncrementCounter(deviceHourKey, TimeSpan.FromHours(1), cancellationToken);
        var deviceDayKey = _keyBuilder.BuildKey(RateLimitScope.Device, deviceFingerprint, "Daily");
        await IncrementCounter(deviceDayKey, TimeSpan.FromDays(1), cancellationToken);
    }

    private async Task<bool> IsLimitExceeded(string key, int limit, CancellationToken cancellationToken)
    {
        var count = await GetCount(key, cancellationToken);
        return count >= limit;
    }

    private async Task<int> GetCount(string key, CancellationToken cancellationToken)
    {
        var countStr = await _cache.GetStringAsync(key, cancellationToken);
        if (int.TryParse(countStr, out var count))
        {
            return count;
        }
        return 0;
    }

    private async Task IncrementCounter(string key, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var countStr = await _cache.GetStringAsync(key, cancellationToken);
        if (int.TryParse(countStr, out var count))
        {
            await _cache.SetStringAsync(key, (count + 1).ToString(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry }, cancellationToken);
        }
        else
        {
            await _cache.SetStringAsync(key, "1", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry }, cancellationToken);
        }
    }
}
