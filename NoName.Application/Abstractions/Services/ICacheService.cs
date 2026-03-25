using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Services
{
    public interface ICacheService
    {

        Task<T?> GetAsync<T>(string key);

        // save data, expiration: time to live (TTL) của cache,null is no expiration
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefix);
        Task<bool> DoWithLockAsync(string lockKey, TimeSpan waitingTime, Func<Task> action);
    }
}
