namespace smartShoppingProject.Application.Abstractions.Notifications;

/// <summary>
/// Tüm kanallar için ortak bildirim isteği. Kullanılacak alanlar kanala göre değişir.
/// </summary>
public sealed record NotificationRequest
{
    public NotificationChannel Channel { get; init; }

    /// <summary>E-posta alıcı adresi (Email kanalı)</summary>
    public string? To { get; init; }

    /// <summary>E-posta konusu (Email kanalı)</summary>
    public string? Subject { get; init; }

    /// <summary>Metin içeriği; e-posta gövdesi, SMS, Discord/Slack mesajı</summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>E-posta için HTML gövde (isteğe bağlı)</summary>
    public string? HtmlBody { get; init; }

    /// <summary>SMS için telefon numarası (Sms kanalı)</summary>
    public string? PhoneNumber { get; init; }

    /// <summary>Discord/Slack webhook URL (config'te varsa boş bırakılabilir)</summary>
    public string? WebhookUrl { get; init; }

    /// <summary>Discord/Slack için başlık (isteğe bağlı)</summary>
    public string? Title { get; init; }
}
