namespace smartShoppingProject.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using smartShoppingProject.Application.Abstractions.Logging;
using smartShoppingProject.Application.Abstractions.Messaging;
using smartShoppingProject.Application.Abstractions.Notifications;
using smartShoppingProject.Application.Abstractions.Persistence;
using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Infrastructure.BackgroundServices;
using smartShoppingProject.Infrastructure.Logging;
using smartShoppingProject.Infrastructure.Messaging;
using smartShoppingProject.Infrastructure.Notifications;
using smartShoppingProject.Infrastructure.Notifications.Options;
using smartShoppingProject.Infrastructure.Persistence;
using smartShoppingProject.Infrastructure.Persistence.Repositories;

/// <summary>
/// Infrastructure katmanı DI kayıtları: DbContext, Repository'ler, UnitOfWork, EventBus, Outbox processor.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' bulunamadı.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        ConfigureEventBus(services, configuration);

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBusinessLogger, BusinessLogger>();

        ConfigureNotifications(services, configuration);

        services.AddHostedService<OutboxMessageProcessor>();

        return services;
    }

    private static void ConfigureNotifications(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSenderOptions>(configuration.GetSection(MailSenderOptions.SectionName));
        services.Configure<SmsSenderOptions>(configuration.GetSection(SmsSenderOptions.SectionName));
        services.Configure<DiscordSenderOptions>(configuration.GetSection(DiscordSenderOptions.SectionName));
        services.Configure<SlackSenderOptions>(configuration.GetSection(SlackSenderOptions.SectionName));
        services.Configure<WhatsAppSenderOptions>(configuration.GetSection(WhatsAppSenderOptions.SectionName));

        services.AddHttpClient();

        services.AddScoped<INotificationSender, MailNotificationSender>();
        services.AddScoped<INotificationSender, SmsNotificationSender>();
        services.AddScoped<INotificationSender, DiscordNotificationSender>();
        services.AddScoped<INotificationSender, SlackNotificationSender>();
        services.AddScoped<INotificationSender, WhatsAppNotificationSender>();
        services.AddScoped<INotificationService, NotificationService>();
    }

    private static void ConfigureEventBus(IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["EventBus:Provider"] ?? "InMemory";

        if (string.Equals(provider, "RabbitMQ", StringComparison.OrdinalIgnoreCase))
        {
            var host = configuration["EventBus:RabbitMQ:Host"] ?? "localhost";
            var vhost = configuration["EventBus:RabbitMQ:VirtualHost"] ?? "/";
            var username = configuration["EventBus:RabbitMQ:Username"] ?? "guest";
            var password = configuration["EventBus:RabbitMQ:Password"] ?? "guest";

            services.AddMassTransit(x =>
            {
                x.AddConsumer<DomainEventEnvelopeConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, vhost, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IEventBus, RabbitMqEventBus>();
        }
        else
        {
            services.AddScoped<IEventBus, InMemoryEventBus>();
        }
    }
}
