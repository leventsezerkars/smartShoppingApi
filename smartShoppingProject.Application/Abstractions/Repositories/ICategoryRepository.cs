namespace smartShoppingProject.Application.Abstractions.Repositories;

using smartShoppingProject.Domain.Entities;

/// <summary>
/// Kategori entity'si için persistence sözleşmesi. Implementation Infrastructure katmanındadır.
/// </summary>
public interface ICategoryRepository
{
    void Add(Category category);
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<CategoryReadModel?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<CategoryReadModel> Items, int TotalCount)> GetCategoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}

/// <summary>
/// Kategori okuma modeli; query tarafında kullanılır.
/// </summary>
public sealed record CategoryReadModel(Guid Id, string Name, bool IsActive, DateTime CreatedAt);
