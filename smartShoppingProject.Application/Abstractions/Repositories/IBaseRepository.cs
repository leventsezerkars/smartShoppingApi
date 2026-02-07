namespace smartShoppingProject.Application.Abstractions.Repositories;

using smartShoppingProject.Domain.Common;

/// <summary>
/// Tüm entity repository'leri için ortak sözleşme. T, Guid Id taşıyan BaseEntity türevidir.
/// Özel senaryolar (okuma modelleri, sayfalama) için ilgili repository kendi metotlarını tanımlar.
/// </summary>
public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(T entity);

    void Update(T entity);

    void Remove(T entity);
}
