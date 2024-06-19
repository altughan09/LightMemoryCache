using System;
using LightMemoryCache.Configuration;
using LightMemoryCache.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace LightMemoryCache.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLightMemoryCache(this IServiceCollection services, Action<CacheOptions> optionsProvider)
    {
        services.Configure(optionsProvider);
        services.AddSingleton<ILightMemoryCache, LightMemoryCache>();
        return services;
    }
}
