using System;
using System.Diagnostics.CodeAnalysis;
using LightMemoryCache.Configuration;
using LightMemoryCache.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace LightMemoryCache.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLightMemoryCache(this IServiceCollection services, Action<CacheOptions>? optionsProvider = null)
    {
        if (optionsProvider != null)
        {
            services.Configure(optionsProvider);
        }
        else
        {
            services.Configure<CacheOptions>(_ => { });
        }

        services.AddSingleton<ILightMemoryCache, LightMemoryCache>();
        return services;
    }
}