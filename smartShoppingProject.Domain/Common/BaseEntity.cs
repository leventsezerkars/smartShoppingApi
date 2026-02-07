namespace smartShoppingProject.Domain.Common;

using smartShoppingProject.Domain.Events;
using smartShoppingProject.Domain.Exceptions;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// EF Core materialization için; türeyen sınıfların parameterless ctor'u bunu çağırır.
    /// </summary>
    protected BaseEntity() { }

    protected BaseEntity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Kimlik boş olamaz.");
        }

        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    protected void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
        {
            throw new DomainException("Domain event null olamaz.");
        }

        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
