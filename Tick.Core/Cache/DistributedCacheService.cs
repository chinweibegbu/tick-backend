using Tick.Core.Contract;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tick.Core.Cache
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IBinarySerializer _binarySerializer;
        private static readonly SemaphoreSlim Semaphore = new(1, 1);

        /// <summary>  </summary>
        public DistributedCacheService(
            IDistributedCache distributedCache,
            IBinarySerializer binarySerializer)
        {
            _distributedCache = distributedCache;
            _binarySerializer = binarySerializer;
        }

#nullable enable
        public async Task<T?> GetAsync<T>(string key) where T : notnull
        {
            var value = await _distributedCache.GetAsync(key);

            return value == default || value.Length == 0 ? default : _binarySerializer.Deserialize<T>(value);
        }
#nullable disable

        /// <summary>  </summary>
        public async Task SetAsync<T>(string key, T value, TimeSpan timeSpan) where T : notnull
        {
            try
            {
                await Semaphore.WaitAsync();
                await _distributedCache.SetAsync(key, _binarySerializer.Serialize(value),
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = timeSpan
                    });
            }
            finally
            {
                Semaphore.Release();
            }
        }


#nullable enable
        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null)
            where T : notnull
        {
            try
            {
                await Semaphore.WaitAsync();
                var value = await _distributedCache.GetAsync(key);

                if (value != default && value.Length != 0)
                    return _binarySerializer.Deserialize<T>(value);


                var valueToCache = await generator();
                await _distributedCache.SetAsync(key, _binarySerializer.Serialize(valueToCache),
                    new DistributedCacheEntryOptions());

                return valueToCache;
            }
            finally
            {
                Semaphore.Release();
            }
        }
#nullable disable

        /// <summary>  </summary>
        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}