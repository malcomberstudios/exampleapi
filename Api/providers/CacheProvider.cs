using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Api.providers
{
    public class CacheProvider
    {
        public delegate Task<T> NoCacheDelegate<T>();
        
        private readonly IMemoryCache _cache;

        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }


        public async Task<T> TryGetValue<T>(string key, NoCacheDelegate<T> noCache, MemoryCacheEntryOptions memoryCacheEntryOptions = null)
        {
            if (_cache.TryGetValue(key, out T cacheEntry)) return cacheEntry;
            
            
            cacheEntry = await noCache();

            var cacheEntryOptions = memoryCacheEntryOptions ?? new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _cache.Set(key, cacheEntry, cacheEntryOptions);

            return cacheEntry;
        }

    }
}