using System;
using System.Diagnostics.CodeAnalysis;

namespace LightMemoryCache.Configuration;

[ExcludeFromCodeCoverage]
public class CacheOptions
{
    public int DefaultExpirationInMinutes { get; set; }
}