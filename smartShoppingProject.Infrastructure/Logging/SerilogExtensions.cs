namespace smartShoppingProject.Infrastructure.Logging;

using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

/// <summary>
/// Serilog + Console + DB (ApplicationLog) konfigürasyonu. Mevcut log mekanizmasıyla entegre.
/// </summary>
public static class SerilogExtensions
{
    public static IHostBuilder UseInfrastructureSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            var minLevel = context.Configuration["Serilog:MinimumLevel"] ?? "Information";
            var level = Enum.TryParse<LogEventLevel>(minLevel, true, out var l) ? l : LogEventLevel.Information;

            configuration
                .MinimumLevel.Is(level)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Sink(new ApplicationLogSink(services));
        });
    }
}
