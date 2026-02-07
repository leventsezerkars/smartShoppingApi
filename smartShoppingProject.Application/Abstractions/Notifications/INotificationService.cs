namespace smartShoppingProject.Application.Abstractions.Notifications;

/// <summary>
/// İstediğiniz kanala (Email, Sms, Discord, Slack) bildirim göndermek için tek giriş noktası.
/// Request.Channel'a göre uygun INotificationSender seçilir ve gönderim yapılır.
/// </summary>
public interface INotificationService
{
    Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default);
}
