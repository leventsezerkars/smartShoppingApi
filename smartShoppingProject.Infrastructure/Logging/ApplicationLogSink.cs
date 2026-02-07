namespace smartShoppingProject.Infrastructure.Logging;

using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using smartShoppingProject.Domain.Entities;
using smartShoppingProject.Infrastructure.Persistence;

/// <summary>
/// Serilog log kayıtlarını ApplicationLog entity ile veritabanına yazar.
/// Mevcut ILogger/Serilog akışıyla entegre çalışır.
/// </summary>
internal sealed class ApplicationLogSink : ILogEventSink
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationLogSink(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exceptionMessage = logEvent.Exception?.ToString();
            var log = ApplicationLog.Create(
                logEvent.Level.ToString(),
                logEvent.RenderMessage(),
                logEvent.Timestamp.UtcDateTime,
                exceptionMessage);

            context.ApplicationLogs.Add(log);
            context.SaveChangesAsync().GetAwaiter().GetResult();
        }
        catch
        {
            // Log yazılamazsa sessizce geç; uygulama akışını bozma
        }
    }
}
