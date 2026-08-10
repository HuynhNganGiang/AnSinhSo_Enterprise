using System;
using System.Collections.Concurrent;

namespace AnSinhSo.Infrastructure.DataImport.Cache;

public class LookupCacheService : ILookupCacheService
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Guid>> _caches = new();

    public void Set<T>(string key, Guid id)
    {
        var cache = _caches.GetOrAdd(typeof(T), _ => new ConcurrentDictionary<string, Guid>());
        cache[key] = id;
    }

    public Guid? Get<T>(string key)
    {
        if (_caches.TryGetValue(typeof(T), out var cache))
        {
            if (cache.TryGetValue(key, out var id))
            {
                return id;
            }
        }

        return null;
    }
}
