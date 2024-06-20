using System.Diagnostics.CodeAnalysis;
using LightMemoryCache.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace LightMemoryCache.Tests;

public class LightMemoryCacheTests
{
    private readonly LightMemoryCache _cache;

    public LightMemoryCacheTests()
    {
        var mockOptions = new Mock<IOptions<CacheOptions>>();
        mockOptions.Setup(x => x.Value).Returns(new CacheOptions { DefaultExpirationInMinutes = 5 });

        _cache = new LightMemoryCache(mockOptions.Object);
    }

    [Fact]
    public void AddAndTryGetValue_ShouldCacheAndRetrieveValue()
    {
        // Arrange
        const string key = "key1";
        const string value = "value1";

        // Act
        _cache.Add(key, value);
        var result = _cache.TryGetValue<string>(key, out var cachedValue);

        // Assert
        Assert.True(result);
        Assert.Equal(value, cachedValue);
    }

    [Fact]
    public void AddAndTryGetValue_ShouldCacheUserObject()
    {
        // Arrange
        const string key = "key1";
        var value = new User { Name = "John Doe" };

        // Act
        _cache.Add(key, value);
        var result = _cache.TryGetValue<User>(key, out var cachedValue);

        // Assert
        Assert.True(result);
        Assert.NotNull(cachedValue);
        Assert.Equal(value, cachedValue);
    }
    
    [Fact]
    public void TryGetValue_ShouldReturnFalseForExpiredItem()
    {
        // Arrange
        const string key = "key3";
        const string value = "value3";
        _cache.Add(key, value, TimeSpan.FromMilliseconds(500));

        // Act
        Thread.Sleep(1000);
        var result = _cache.TryGetValue<string>(key, out var cachedValue);

        // Assert
        Assert.False(result);
        Assert.Null(cachedValue);
    }


    [Fact]
    public void TryGetValue_ShouldReturnFalseForNonExistentKey()
    {
        // Arrange
        const string key = "nonexistentKey";

        // Act
        var result = _cache.TryGetValue<string>(key, out var cachedValue);

        // Assert
        Assert.False(result);
        Assert.Null(cachedValue);
    }

    [Fact]
    public void InvalidateByKey_ShouldRemoveCachedItemsByKey()
    {
        // Arrange
        const string key1 = "key1";
        const string key2 = "key2";
        const string value1 = "value1";
        const string value2 = "value2";

        _cache.Add(key1, value1);
        _cache.Add(key2, value2);

        // Act
        _cache.InvalidateByKey("key1");

        // Assert
        Assert.False(_cache.TryGetValue<string>(key1, out _));
        Assert.True(_cache.TryGetValue<string>(key2, out _));
    }

    [Fact]
    public void Invalidate_ShouldRemoveAllCachedItems()
    {
        // Arrange
        const string key1 = "key1";
        const string key2 = "key2";
        const string value1 = "value1";
        const string value2 = "value2";

        _cache.Add(key1, value1);
        _cache.Add(key2, value2);

        // Act
        _cache.Invalidate();

        // Assert
        Assert.False(_cache.TryGetValue<string>(key1, out _));
        Assert.False(_cache.TryGetValue<string>(key2, out _));
    }

    [ExcludeFromCodeCoverage]
    private class User
    {
        public string Name { get; set; } = default!;
    }
}