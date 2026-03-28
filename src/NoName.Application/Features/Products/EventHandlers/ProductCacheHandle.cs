using MediatR;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Products.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.EventHandlers
{
    public class ProductCacheHandler : INotificationHandler<ProductChangedEvent>
    {
        private readonly ICacheService _cacheService;

        public ProductCacheHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task Handle(ProductChangedEvent notification, CancellationToken ct)
        {
            var tasks = new List<Task>{
                _cacheService.RemoveAsync(CacheKeys.ProductPrefix(notification.ProductId)),
                _cacheService.RemoveAsync(CacheKeys.ProductAllListPrefix())};

            await Task.WhenAll(tasks);
        }

    }
}
