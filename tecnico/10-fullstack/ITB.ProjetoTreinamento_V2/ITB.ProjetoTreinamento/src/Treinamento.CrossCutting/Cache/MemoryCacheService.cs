using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Treinamento.Domain.Core.Cache;

namespace Treinamento.CrossCutting.Cache;

public class MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger) : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
    {
        cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
        };
        cache.Set(key, value, options);
        logger.LogDebug("Valor armazenado no cache. Key={Key}", key);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern) => Task.CompletedTask;

    public Task<bool> ExistsAsync(string key) => Task.FromResult(cache.TryGetValue(key, out _));

    public Task ClearAsync()
    {
        if (cache is MemoryCache memoryCache)
            memoryCache.Compact(1.0);
        return Task.CompletedTask;
    }
}
