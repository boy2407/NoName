using NoName.Application.Abstractions.Services;
using RedLockNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Services
{
    public class DistributedLockService : IDistributedLockService
    {
        private readonly IDistributedLockFactory _lockFactory;

        public DistributedLockService(IDistributedLockFactory lockFactory)
            => _lockFactory = lockFactory;

        public async Task<IDisposable> AcquireLockAsync(List<string>keys, TimeSpan expiry, TimeSpan wait, TimeSpan retry)
        {
            var acquiredLocks = new List<IDisposable>();

            foreach (var k in keys)
            {
                var result = await _lockFactory.CreateLockAsync(k, expiry, wait,retry);
                if (result.IsAcquired)
                    acquiredLocks.Add(result);
                else
                {
                    foreach (var l in acquiredLocks) l.Dispose();
                    Console.WriteLine("===============================================");
                    Console.WriteLine($": {k}");
                    Console.WriteLine("===============================================");
                    throw new Exception($"Sản phẩm {k} đang bận, thử lại sau!");
                }
                   
             
            }
            return new MultiLockDisposer(acquiredLocks);
        }
    }


    public class MultiLockDisposer : IDisposable
    {
        private readonly List<IDisposable> _locks;
        public MultiLockDisposer(List<IDisposable> locks) => _locks = locks;
        public void Dispose()
        {
          
            foreach (var l in _locks) l?.Dispose();
        }
    }
}
