namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Common;
using smartShoppingProject.Domain.Exceptions;

public sealed class Category : BaseEntity
{
    /// <summary>
    /// EF Core materialization için; iş kuralları için kullanılmaz.
    /// </summary>
    private Category() { }

    public Category(Guid id, string name, bool isActive = true)
        : base(id)
    {
        Name = EnsureName(name);
        IsActive = isActive;
    }

    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    public void Rename(string name)
    {
        Name = EnsureName(name);
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        SetUpdatedAt();
    }

    private static string EnsureName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Kategori adı zorunludur.");
        }

        return name.Trim();
    }
}
