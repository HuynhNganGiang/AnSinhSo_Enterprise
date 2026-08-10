using System;

namespace AnSinhSo.Infrastructure.DataImport.Cache;

public interface ILookupCacheService
{
    void Set<T>(string key, Guid id);
    Guid? Get<T>(string key);
}
