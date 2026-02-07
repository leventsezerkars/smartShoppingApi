namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Exceptions;

/// <summary>
/// Outbox pattern için domain entity. Event kaydı oluşturma ve işlendi işaretleme davranışı burada.
/// Infrastructure yalnızca persistence yapar; kurallar Domain'de kalır.
/// </summary>
public sealed class OutboxMessage
{
    private OutboxMessage(Guid id, string type, string payload, DateTime occurredOn)
    {
        if (id == Guid.Empty)
            throw new DomainException("Outbox mesaj kimliği boş olamaz.");
        if (string.IsNullOrWhiteSpace(type))
            throw new DomainException("Outbox mesaj tipi zorunludur.");
        if (payload is null)
            throw new DomainException("Outbox payload zorunludur.");

        Id = id;
        Type = type.Trim();
        Payload = payload;
        OccurredOn = occurredOn;
    }

    /// <summary>
    /// EF Core materialization için.
    /// </summary>
    private OutboxMessage() { }

    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    public static OutboxMessage Create(string type, string payload, DateTime occurredOn)
    {
        return new OutboxMessage(Guid.NewGuid(), type, payload, occurredOn);
    }

    public void MarkProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }

    public bool IsProcessed => ProcessedOn.HasValue;
}
