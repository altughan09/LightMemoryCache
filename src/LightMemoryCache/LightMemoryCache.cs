using System;
using System.Collections.Concurrent;
using System.Threading;
using LightMemoryCache.Configuration;
using LightMemoryCache.Contracts;
using LightMemoryCache.Entities;
using Microsoft.Extensions.Options;

namespace LightMemoryCache;

public class LightMemoryCache : ILightMemoryCache, IDisposable
{
    private readonly ConcurrentDictionary<string, CacheItem> _cache = new();
    private readonly TimeSpan _defaultExpiration;
    private readonly Timer _cleanupTimer;
    private bool _disposed;

    public LightMemoryCache(IOptions<CacheOptions> options)
    {
        _defaultExpiration = TimeSpan.FromMinutes(options.Value.DefaultExpirationInMinutes);
        _cleanupTimer = new Timer(CleanupExpiredItems, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    public void Add<TValue>(string key, TValue value, TimeSpan? expiration = null)
    {
        if (value == null) return;
        var expiry = expiration ?? _defaultExpiration;
        var cacheItem = new CacheItem(value, DateTime.UtcNow.Add(expiry));
        _cache[key] = cacheItem;
    }

    public bool TryGetValue<TValue>(string key, out TValue? value)
    {
        if (_cache.TryGetValue(key, out var cacheItem) && DateTime.UtcNow < cacheItem.Expiration)
        {
            value = (TValue)cacheItem.Value;
            return true;
        }

        InvalidateByKey(key);
        value = default;
        return false;
    }

    public void InvalidateByKey(string key)
    {
        _cache.TryRemove(key, out _);
    }

    public void Invalidate()
    {
        _cache.Clear();
    }

    private void CleanupExpiredItems(object? state)
    {
        var now = DateTime.UtcNow;
        foreach (var key in _cache.Keys)
        {
            if (_cache.TryGetValue(key, out var cacheItem) && now >= cacheItem.Expiration)
            {
                _cache.TryRemove(key, out _);
            }
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _cleanupTimer.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
