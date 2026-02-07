namespace smartShoppingProject.Infrastructure.Messaging;

/// <summary>
/// MassTransit ile RabbitMQ'ya publish edilecek domain event sarmalayıcı.
/// Application/Domain MassTransit'e bağımlı olmaz.
/// </summary>
public sealed record DomainEventEnvelope(string Type, string Payload, DateTime OccurredOn);
