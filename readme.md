# LightMemoryCache

LightMemoryCache is a lightweight, thread-safe, in-memory caching library for .NET applications. It provides a simple and efficient way to cache data objects with customizable expiration times.

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

## Benchmark

### LightMemoryCache vs MemoryCache

Both LightMemoryCache and MemoryCache were benchmarked under similar conditions to evaluate their performance:

| Method                                       |     Mean |    Error |   StdDev |
| -------------------------------------------- | -------: | -------: | -------: |
| LightMemoryCache_AddAndRetrieve              | 12.19 us | 0.029 us | 0.027 us |
| MemoryCache_AddAndRetrieve                   | 13.60 us | 0.149 us | 0.116 us |
| LightMemoryCache_AddAndRetrieveComplexObject | 23.83 us | 0.093 us | 0.072 us |
| MemoryCache_AddAndRetrieveComplexObject      | 34.23 us | 0.083 us | 0.077 us |
