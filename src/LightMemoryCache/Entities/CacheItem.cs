using System;

namespace LightMemoryCache.Entities;

public class CacheItem
{
    public CacheItem(object value, DateTime expiration)
    {
        Value = value;
        Expiration = expiration;
    }

    public object Value { get; }
    public DateTime Expiration { get; }
}
