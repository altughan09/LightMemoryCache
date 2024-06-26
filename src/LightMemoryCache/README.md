# LightMemoryCache

LightMemoryCache is a lightweight, thread-safe, in-memory caching library for .NET applications. It provides a simple and efficient way to cache data objects with customizable expiration times.

[![Build and Test](https://github.com/altughan09/LightMemoryCache/actions/workflows/ci.yml/badge.svg)](https://github.com/altughan09/LightMemoryCache/actions/workflows/ci.yml)
[![Coverage Status](https://coveralls.io/repos/github/altughan09/LightMemoryCache/badge.svg?branch=main)](https://coveralls.io/github/altughan09/LightMemoryCache?branch=main)
[![NuGet](https://buildstats.info/nuget/LightMemoryCache)](http://www.nuget.org/packages/LightMemoryCache)

## Features

- **Simple API**: Easy-to-use methods for adding, retrieving, and invalidating cached items.
- **Thread-Safe**: Utilizes ConcurrentDictionary along with the lock statement for thread safety.
- **Expiration**: Supports customizable expiration times for cached items.
- **Automatic Cleanup**: Periodically cleans up expired items using a background timer.
- **Configurable**: Allows configuration of default expiration time via options.

## Installation

You can install LightMemoryCache via NuGet Package Manager:

```bash
Install-Package LightMemoryCache
```

or dotnet CLI

```bash
dotnet add package LightMemoryCache
```

## Usage

Register the component:

```C#
services.AddLightMemoryCache();
```

or

```C#
var configurationRoot = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .Build();

var defaultExpirationInMinutes = configurationRoot.GetValue<int>("CacheOptions:DefaultExpirationInMinutes");

services.AddLightMemoryCache(options => { options.DefaultExpirationInMinutes = defaultExpirationInMinutes; });
```

appsettings.json

```json
"CacheOptions": {
    "DefaultExpirationInMinutes": "15"
}
```

Dependency injection

```C#
public class CountryService : ICountryService
{
    private readonly ILightMemoryCache _cache;
    private readonly ICountryRepository _countryRepository;
    private const string CountryCacheKey = "AllCountries";

    public CountryService(ILightMemoryCache cache, ICountryRepository countryRepository)
    {
        _cache = cache;
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        if (_cache.TryGetValue(CountryCacheKey, out IEnumerable<CountryDto> cachedCountries))
        {
            return cachedCountries;
        }

        var countries = await _countryRepository.GetAllAsync();
        _cache.Add(CountryCacheKey, countries, TimeSpan.FromMinutes(30));
        return countries;
    }
    // ...
}
```

## Benchmark

### LightMemoryCache vs MemoryCache

Both LightMemoryCache and MemoryCache were benchmarked under similar conditions to evaluate their performance:

| Method                                       |     Mean |    Error |   StdDev |
| -------------------------------------------- | -------: | -------: | -------: |
| LightMemoryCache_AddAndRetrieve              | 12.19 us | 0.029 us | 0.027 us |
| MemoryCache_AddAndRetrieve                   | 13.60 us | 0.149 us | 0.116 us |
| LightMemoryCache_AddAndRetrieveComplexObject | 23.83 us | 0.093 us | 0.072 us |
| MemoryCache_AddAndRetrieveComplexObject      | 34.23 us | 0.083 us | 0.077 us |
