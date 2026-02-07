namespace smartShoppingProject.Application.Abstractions.Notifications;

/// <summary>
/// Tek bir kanal için bildirim gönderen implementasyon. Her kanal (Mail, Sms, Discord, Slack) kendi sender'ına sahiptir.
/// </summary>
public interface INotificationSender
{
    NotificationChannel SupportedChannel { get; }

    Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default);
}
