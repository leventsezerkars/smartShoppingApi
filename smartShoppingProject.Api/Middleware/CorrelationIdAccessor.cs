namespace smartShoppingProject.Api.Middleware;

using smartShoppingProject.Application.Abstractions.Logging;

/// <summary>
/// CorrelationId'yi HttpContext.Items üzerinden okur; middleware tarafından set edilir.
/// </summary>
public sealed class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentCorrelationId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.Items.TryGetValue(CorrelationIdMiddleware.CorrelationIdItemKey, out var value) == true
            && value is string id)
            return id;
        return null;
    }
}
