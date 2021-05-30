using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ShopApi.Core.Extensions
{
    public static class CacheExtensions
    {
        public static async Task<T> GetDataAsync<T>(this IDistributedCache cache, string cacheKey)
        {
            string data = await cache.GetStringAsync(cacheKey);
            if (string.IsNullOrWhiteSpace(data))
                return default;

            return JsonConvert.DeserializeObject<T>(data);
        }

        public static async Task AddDataAsync<T>(this IDistributedCache cache, string cacheKey, T data,
                                      TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                // Item will live for a total of 1 minute in the cache
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),

                // If the cached item is not used within the unusedExpireTime, 
                // then it will be expired. Even if the AbsoluteExpirationRelativeToNow has not been met
                SlidingExpiration = unusedExpireTime
            };

            // move to AppSettings.cs
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string jsonData = JsonConvert.SerializeObject(data, settings);

            await cache.SetStringAsync(cacheKey, jsonData, options);
        }
    }
}
