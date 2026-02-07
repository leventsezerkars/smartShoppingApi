namespace smartShoppingProject.Api.Middleware;

using Serilog.Context;

/// <summary>
/// Her istek için CorrelationId üretir veya header'dan okur; hem HttpContext hem Serilog LogContext'e yazar.
/// Teknik loglar (Serilog) ve BusinessLog aynı id ile ilişkilendirilir.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    public const string CorrelationIdItemKey = "CorrelationId";
    public const string CorrelationIdHeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault()
            ?? Guid.NewGuid().ToString("N");

        context.Items[CorrelationIdItemKey] = correlationId;
        context.Response.Headers.Append(CorrelationIdHeaderName, correlationId);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
