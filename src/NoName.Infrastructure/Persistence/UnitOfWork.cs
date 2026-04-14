using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Infrastructure.EF;

namespace NoName.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly NoNameDbContext _context;
    public IProductRepository Products { get; }
    public ICartRepository Carts { get; }
    public IOrderRepository Orders { get; }
    public ILanguageRepository Languages { get; }
    public ICategoryRepository Categories { get; }
    public ITransactionRepository Transactions { get; }
    public IProductVariantRepository ProductVariants { get; }


    public UnitOfWork (NoNameDbContext context, IProductRepository products, ICartRepository carts, IOrderRepository orders, ILanguageRepository languages,
                                               ICategoryRepository categories, ITransactionRepository transactions, IProductVariantRepository productVariants)
    {
        _context = context;
        Products = products;
        Carts = carts;
        Orders = orders;
        Languages = languages;
        Categories = categories;
        Transactions = transactions;
        ProductVariants = productVariants;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}