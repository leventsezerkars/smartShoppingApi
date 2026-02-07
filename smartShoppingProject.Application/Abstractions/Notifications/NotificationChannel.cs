namespace smartShoppingProject.Application.Abstractions.Notifications;

/// <summary>
/// Bildirim gönderilebilecek kanallar. Yeni kanal eklenince bu enum ve ilgili INotificationSender implementasyonu eklenir.
/// </summary>
public enum NotificationChannel
{
    Email,
    Sms,
    Discord,
    Slack,
    WhatsApp
}
