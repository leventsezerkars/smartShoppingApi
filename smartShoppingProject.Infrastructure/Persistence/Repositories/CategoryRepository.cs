namespace smartShoppingProject.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Domain.Entities;
using Persistence;

internal sealed class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<CategoryReadModel?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryReadModel(c.Id, c.Name, c.IsActive, c.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<CategoryReadModel> Items, int TotalCount)> GetCategoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Categories.AsNoTracking().OrderBy(c => c.Name);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CategoryReadModel(c.Id, c.Name, c.IsActive, c.CreatedAt))
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
