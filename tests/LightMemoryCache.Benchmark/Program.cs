using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LightMemoryCache.Configuration;
using LightMemoryCache.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace LightMemoryCache.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CacheBenchmark>();
        Console.WriteLine(summary);
    }
}

public class CacheBenchmark
{
    private ILightMemoryCache? _lightMemoryCache;
    private IMemoryCache? _memoryCache;

    [GlobalSetup]
    public void Setup()
    {
        var options = Options.Create(new CacheOptions { DefaultExpirationInMinutes = 5 });
        _lightMemoryCache = new LightMemoryCache(options);

        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    [Benchmark]
    public void LightMemoryCache_AddAndRetrieve()
    {
        const string baseKey = "key";
        
        for (var i = 0; i < 100; i++)
        {
            var key = $"{baseKey}_{i}";
            var value = $"Test Object {i + 1}";
            
            _lightMemoryCache!.Add(key, value);
            _lightMemoryCache.TryGetValue<string>(key, out _);
        }
    }

    [Benchmark]
    public void MemoryCache_AddAndRetrieve()
    {
        const string baseKey = "key";
        
        for (var i = 0; i < 100; i++)
        {
            var key = $"{baseKey}_{i}";
            var value = $"Test Object {i + 1}";
            
            _memoryCache!.Set(key, value);
            _memoryCache!.TryGetValue<string>(key, out _);
        }
    }
        
    [Benchmark]
    public void LightMemoryCache_AddAndRetrieveComplexObject()
    {
        const string baseKey = "complexKey";
        
        for (var i = 0; i < 100; i++)
        {
            var key = $"{baseKey}_{i}";
            var complexObject = new ComplexObject
            {
                Id = i + 1,
                Name = $"Test Object {i + 1}",
                Timestamp = DateTime.UtcNow,
                Data = [$"Item{i + 1}_1", $"Item{i + 1}_2", $"Item{i + 1}_3"]
            };

            _lightMemoryCache!.Add(key, complexObject);
            _lightMemoryCache!.TryGetValue<ComplexObject>(key, out _);
        }
    }

    [Benchmark]
    public void MemoryCache_AddAndRetrieveComplexObject()
    {
        const string baseKey = "complexKey";
        
        for (var i = 0; i < 100; i++)
        {
            var key = $"{baseKey}_{i}";
            var complexObject = new ComplexObject
            {
                Id = i + 1,
                Name = $"Test Object {i + 1}",
                Timestamp = DateTime.UtcNow,
                Data = [$"Item{i + 1}_1", $"Item{i + 1}_2", $"Item{i + 1}_3"]
            };

            _memoryCache!.Set(key, complexObject);
            _memoryCache!.TryGetValue<ComplexObject>(key , out _);
        }
    }
    
    private class ComplexObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime Timestamp { get; set; }
        public List<string> Data { get; set; } = [];
    }
}