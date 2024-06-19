using System;

namespace LightMemoryCache.Contracts;

public interface ILightMemoryCache
{
    void Add<TValue>(string key, TValue value, TimeSpan? expiration = null);
    bool TryGetValue<TValue>(string key, out TValue? value);
    void InvalidateByKey(string key);
    void Invalidate();
}