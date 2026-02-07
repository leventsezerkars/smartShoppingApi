namespace smartShoppingProject.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
