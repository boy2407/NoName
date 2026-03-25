using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedLockNet;
using StackExchange.Redis;
using System.Text.Json;
using NoName.Application.Abstractions.Services;

namespace NoName.Infrastructure.Services
{

    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly IDistributedLockFactory _lockFactory;
        private readonly IConnectionMultiplexer _redis;
        public RedisCacheService(IConnectionMultiplexer redis, IDistributedLockFactory lockFactory)
        {
            _redis = redis;
            _db = redis.GetDatabase();
            _lockFactory = lockFactory;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return  JsonSerializer.Deserialize<T>((string)value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = expiration ?? TimeSpan.FromHours(1);
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
        public async Task RemoveByPrefixAsync(string prefix)
        {
            var endpoints = _redis.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                var keys = server.Keys(pattern: $"{prefix}*").ToArray();

                if (keys.Any())
                {
                    await _db.KeyDeleteAsync(keys); 
                }
            }
        }
        public async Task<bool> DoWithLockAsync(string lockKey, TimeSpan waitingTime, Func<Task> action)
        {
            using (var redLock = await _lockFactory.CreateLockAsync(lockKey, TimeSpan.FromSeconds(30), waitingTime, TimeSpan.FromSeconds(1)))
            {
                if (redLock.IsAcquired)
                {
                    await action();
                    return true;
                }
                return false;
            }
        }




    }
}
