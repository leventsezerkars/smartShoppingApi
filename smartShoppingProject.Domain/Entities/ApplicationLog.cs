namespace smartShoppingProject.Domain.Entities;

using smartShoppingProject.Domain.Exceptions;

/// <summary>
/// Uygulama log kaydı; DB'ye yazılacak loglar için domain entity.
/// Seviye ve mesaj invariant'ları burada korunur.
/// </summary>
public sealed class ApplicationLog
{
    private const int MessageMaxLength = 4000;
    private const int ExceptionMaxLength = 2000;

    private ApplicationLog(Guid id, string level, string message, DateTime createdAt, string? exceptionMessage)
    {
        if (id == Guid.Empty)
            throw new DomainException("Log kimliği boş olamaz.");
        if (string.IsNullOrWhiteSpace(level))
            throw new DomainException("Log seviyesi zorunludur.");
        if (message is null)
            throw new DomainException("Log mesajı zorunludur.");

        Id = id;
        Level = level.Trim();
        Message = message.Length > MessageMaxLength ? message[..MessageMaxLength] : message;
        CreatedAt = createdAt;
        ExceptionMessage = exceptionMessage is null ? null : (exceptionMessage.Length > ExceptionMaxLength ? exceptionMessage[..ExceptionMaxLength] : exceptionMessage);
    }

    /// <summary>
    /// EF Core materialization için.
    /// </summary>
    private ApplicationLog() { }

    public Guid Id { get; private set; }
    public string Level { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public string? ExceptionMessage { get; private set; }

    public static ApplicationLog Create(string level, string message, DateTime? createdAt = null, string? exceptionMessage = null)
    {
        return new ApplicationLog(
            Guid.NewGuid(),
            level,
            message,
            createdAt ?? DateTime.UtcNow,
            exceptionMessage);
    }
}
