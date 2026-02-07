namespace smartShoppingProject.Application.Abstractions.Logging;

/// <summary>
/// Mevcut HTTP isteğinin CorrelationId değerine erişim. Middleware tarafından set edilir.
/// Hem Serilog (teknik log) hem BusinessLog için aynı id kullanılır.
/// </summary>
public interface ICorrelationIdAccessor
{
    /// <summary>
    /// İstek kapsamındaki correlation id; yoksa null.
    /// </summary>
    string? GetCurrentCorrelationId();
}
