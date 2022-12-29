using PostBoy.Core.Ioc;
using PostBoy.Messages.Enums.Caching;

namespace PostBoy.Core.Services.Caching;

public interface ICacheManager : IScopedDependency
{
    Task<T> UsingCacheAsync<T>(string key, Func<string, Task<T>> whenNotFound, TimeSpan? expiry = null,
        CancellationToken cancellationToken = default) where T : class;

    Task<T> GetOrAddAsync<T>(string key, Func<string, Task<T>> whenNotFound, CachingType cachingType,
        TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class;
}

public class CacheManager : ICacheManager
{
    private readonly RedisCacheService _redisCacheService;
    private readonly MemoryCacheService _memoryCacheService;

    public CacheManager(RedisCacheService redisCacheService, MemoryCacheService memoryCacheService)
    {
        _redisCacheService = redisCacheService;
        _memoryCacheService = memoryCacheService;
    }

    public async Task<T> UsingCacheAsync<T>(string key, Func<string, Task<T>> whenNotFound, 
        TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
    {
        var result = await _memoryCacheService.GetAsync<T>(key, cancellationToken).ConfigureAwait(false);
        
        if (result != null)
            return result;

        expiry ??= TimeSpan.FromMinutes(5);
        
        result = await GetOrAddAsync(key, whenNotFound, CachingType.RedisCache, expiry, cancellationToken).ConfigureAwait(false);
        
        await _memoryCacheService.SetAsync(key, result, expiry.Value, cancellationToken).ConfigureAwait(false);
        
        return result;
    }
    
    public async Task<T> GetOrAddAsync<T>(string key, Func<string, Task<T>> whenNotFound, CachingType cachingType,
        TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T: class
    {
        ICachingService cachingService = cachingType switch
        {
            CachingType.RedisCache => _redisCacheService,
            CachingType.MemoryCache => _memoryCacheService
        };

        var cachedResult = await cachingService.GetAsync<T>(key, cancellationToken).ConfigureAwait(false);

        if (cachedResult != null)
            return cachedResult;

        var result = await whenNotFound(key);

        await cachingService.SetAsync(key, result, expiry, cancellationToken).ConfigureAwait(false);

        return result;
    }
}