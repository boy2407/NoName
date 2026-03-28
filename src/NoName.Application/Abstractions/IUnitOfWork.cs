using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        ICategoryRepository Categories { get; }
        ILanguageRepository Languages { get; }
        IProductVariantRepository ProductVariants { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
