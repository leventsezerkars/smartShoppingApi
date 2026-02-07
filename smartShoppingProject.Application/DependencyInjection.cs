namespace smartShoppingProject.Application;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using smartShoppingProject.Application.Behaviors;
using System.Reflection;

/// <summary>
/// Application katmanı DI kayıtları. MediatR, pipeline behavior'lar, FluentValidation.
/// Infrastructure referansı eklenmez; interface'ler burada tanımlı, implementasyon API/Infrastructure'da register edilir.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }
}
