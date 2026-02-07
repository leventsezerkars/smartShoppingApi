namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Exceptions;

/// <summary>
/// İş / audit log kaydı. "Sistemin ne yaptığını" anlatır; teknik loglardan (ApplicationLog) ayrıdır.
/// </summary>
public sealed class BusinessLog
{
    private const int EntityNameMaxLength = 128;
    private const int ActionMaxLength = 128;
    private const int ContextJsonMaxLength = 4000;
    private const int CorrelationIdMaxLength = 64;

    private BusinessLog(Guid id, string entityName, Guid? entityId, string action, string? contextJson, string? correlationId, DateTime createdAtUtc)
    {
        if (id == Guid.Empty)
            throw new DomainException("BusinessLog kimliği boş olamaz.");
        if (string.IsNullOrWhiteSpace(entityName))
            throw new DomainException("EntityName zorunludur.");
        if (string.IsNullOrWhiteSpace(action))
            throw new DomainException("Action zorunludur.");

        Id = id;
        EntityName = Truncate(entityName.Trim(), EntityNameMaxLength);
        EntityId = entityId;
        Action = Truncate(action.Trim(), ActionMaxLength);
        ContextJson = contextJson is null ? null : Truncate(contextJson, ContextJsonMaxLength);
        CorrelationId = correlationId is null ? null : Truncate(correlationId, CorrelationIdMaxLength);
        CreatedAtUtc = createdAtUtc;
    }

    /// <summary>
    /// EF Core materialization için.
    /// </summary>
    private BusinessLog() { }

    public Guid Id { get; private set; }
    public string EntityName { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string? ContextJson { get; private set; }
    public string? CorrelationId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static BusinessLog Create(string entityName, Guid? entityId, string action, string? contextJson, string? correlationId, DateTime createdAtUtc)
    {
        return new BusinessLog(
            Guid.NewGuid(),
            entityName,
            entityId,
            action,
            contextJson,
            correlationId,
            createdAtUtc);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
