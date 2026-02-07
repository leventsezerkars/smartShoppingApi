namespace smartShoppingProject.Infrastructure.Logging;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Domain.Entities;
using smartShoppingProject.Infrastructure.Persistence;

/// <summary>
/// İş / audit loglarını yalnızca BusinessLogs tablosuna yazar. Serilog veya ILogger kullanmaz.
/// </summary>
public sealed class BusinessLogger : IBusinessLogger
{
    private readonly AppDbContext _context;
    private readonly ICorrelationIdAccessor _correlationIdAccessor;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = false };

    public BusinessLogger(AppDbContext context, ICorrelationIdAccessor correlationIdAccessor)
    {
        _context = context;
        _correlationIdAccessor = correlationIdAccessor;
    }

    public async Task LogBusinessEventAsync(
        string entityName,
        Guid? entityId,
        string action,
        object? context,
        string? correlationId,
        DateTime createdAtUtc,
        CancellationToken cancellationToken = default)
    {
        var effectiveCorrelationId = correlationId ?? _correlationIdAccessor.GetCurrentCorrelationId();
        var contextJson = context is null ? null : JsonSerializer.Serialize(context, JsonOptions);

        var log = BusinessLog.Create(
            entityName,
            entityId,
            action,
            contextJson,
            effectiveCorrelationId,
            createdAtUtc);

        await _context.BusinessLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
