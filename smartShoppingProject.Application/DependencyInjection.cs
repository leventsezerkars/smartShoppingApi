namespace smartShoppingProject.Application;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using smartShoppingProject.Application.Abstractions.Events;
using smartShoppingProject.Application.Behaviors;
using smartShoppingProject.Application.Events;
using System.Reflection;

/// <summary>
/// Application katmanı DI kayıtları. MediatR, pipeline behavior'lar, FluentValidation, event mapper.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IDomainEventToNotificationMapper, DomainEventToNotificationMapper>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }
}
