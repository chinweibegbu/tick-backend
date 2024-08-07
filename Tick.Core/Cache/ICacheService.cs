using System;
using System.Threading.Tasks;

namespace Tick.Core.Cache
{
    public interface ICacheService
    {
#nullable enable
        /// <summary> Get from cache the T value. </summary>
        Task<T?> GetAsync<T>(string key) where T : notnull;

        /// <summary> Save into cache the T value. </summary>
        Task SetAsync<T>(string key, T value, TimeSpan timeSpan) where T : notnull;

        /// <summary> Validate if key exists on cache, and if not, call generator function and cache response. </summary>
        Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null) where T : notnull;

        /// <summary> Remove cache entry. </summary>
        void Remove(string key);
#nullable disable
    }
}