namespace smartShoppingProject.Application.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// Sadece teknik hata (exception) loglanır. Giriş/çıkış ve iş logları atılmaz;
/// business anlamı olan noktalar IBusinessLogger ile handler içinde loglanır.
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hata: {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}
