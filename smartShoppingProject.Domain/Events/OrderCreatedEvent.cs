namespace smartShoppingProject.Domain.Events;

// Domain event'ler ödeme, stok, bildirim gibi yan etkileri çekirdek modelden ayırır; loglama ve izlenebilirlik için anlamlı mesajlar kullanılır.
public sealed record OrderCreatedEvent(Guid OrderId, DateTime OccurredAt) : IDomainEvent
{
    public DateTime OccurredOn => OccurredAt;
}
