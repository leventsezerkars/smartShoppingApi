namespace smartShoppingProject.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Domain.Common;
using Persistence;

/// <summary>
/// Generic repository; yalnızca GetById, Add, Update, Remove. Özel repository'ler bu sınıftan türer.
/// </summary>
internal class Repository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
