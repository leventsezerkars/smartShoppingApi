namespace smartShoppingProject.Application.Abstractions.Logging;

/// <summary>
/// İş / audit logları için abstraction. Teknik loglar (Serilog/ILogger) ile karıştırılmaz.
/// "Sistemin ne yaptığını" anlatır; exception ve pipeline logları ILogger'da kalır.
/// </summary>
public interface IBusinessLogger
{
    /// <summary>
    /// İş anlamı olan bir olayı BusinessLogs tablosuna yazar.
    /// </summary>
    /// <param name="entityName">İlgili aggregate/entity adı (örn. Order, Product)</param>
    /// <param name="entityId">İlgili kayıt kimliği; yoksa null</param>
    /// <param name="action">Yapılan işlem (örn. OrderCreated, PaymentFailed)</param>
    /// <param name="context">Ek bağlam; JSON'a serialize edilir</param>
    /// <param name="correlationId">İstek takip kimliği; middleware'den gelir</param>
    /// <param name="createdAtUtc">Olay zamanı (UTC)</param>
    Task LogBusinessEventAsync(
        string entityName,
        Guid? entityId,
        string action,
        object? context,
        string? correlationId,
        DateTime createdAtUtc,
        CancellationToken cancellationToken = default);
}
