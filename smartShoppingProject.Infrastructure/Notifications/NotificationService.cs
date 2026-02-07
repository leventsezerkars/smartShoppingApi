namespace smartShoppingProject.Infrastructure.Notifications;

using Microsoft.Extensions.Logging;
using smartShoppingProject.Application.Abstractions.Notifications;

/// <summary>
/// İstenen kanala göre uygun INotificationSender'ı seçer ve gönderimi yapar.
/// Yeni kanal eklemek için yeni bir INotificationSender implementasyonu ve DI kaydı yeterli.
/// </summary>
public sealed class NotificationService : INotificationService
{
    private readonly IEnumerable<INotificationSender> _senders;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IEnumerable<INotificationSender> senders, ILogger<NotificationService> logger)
    {
        _senders = senders;
        _logger = logger;
    }

    public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        var sender = _senders.FirstOrDefault(s => s.SupportedChannel == request.Channel);
        if (sender is null)
        {
            _logger.LogWarning("Kanal için sender bulunamadı: {Channel}", request.Channel);
            return;
        }

        await sender.SendAsync(request, cancellationToken);
    }
}
