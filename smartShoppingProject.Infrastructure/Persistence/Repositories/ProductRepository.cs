namespace smartShoppingProject.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Domain.Entities;
using Persistence;

internal sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<ProductReadModel?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .Select(p => new ProductReadModel(
                p.Id,
                p.Name,
                p.Description,
                p.Price.Amount,
                p.Price.Currency.ToString(),
                p.StockQuantity,
                p.IsActive,
                p.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<ProductReadModel> Items, int TotalCount)> GetProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsNoTracking().OrderBy(p => p.Name);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductReadModel(
                p.Id,
                p.Name,
                p.Description,
                p.Price.Amount,
                p.Price.Currency.ToString(),
                p.StockQuantity,
                p.IsActive,
                p.CreatedAt))
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
