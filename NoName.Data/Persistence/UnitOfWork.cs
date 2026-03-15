using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Infrastructure.EF;

namespace NoName.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly NoNameDbContext _context;

    public IProductRepository Products { get; }
    public ILanguageRepository Languages { get; }
    public ICategoryRepository Categories { get; }

    public UnitOfWork(NoNameDbContext context,IProductRepository products,ILanguageRepository languages,ICategoryRepository categories)
    {
        _context = context;
        Products = products;
        Languages = languages;
        Categories = categories;
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