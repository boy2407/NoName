using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<ProductVariant>> GetByProductIdAsync(int productId, CancellationToken ct);
        Task<List<string>> GetExistingSKUsAsync(List<string> SKUs, CancellationToken ct);
        void Add(ProductVariant variant);
        void Remove(ProductVariant variant);
       
    }
}
