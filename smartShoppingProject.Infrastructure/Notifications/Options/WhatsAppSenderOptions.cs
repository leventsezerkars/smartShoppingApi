namespace smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// WhatsApp bildirimleri. appsettings: Notifications:WhatsApp
/// ApiUrl: Kullanılan gateway'in endpoint'i (örn. Twilio, iletimerkezi vb.).
/// ApiKey: İsteğe bağlı Authorization header değeri.
/// </summary>
public sealed class WhatsAppSenderOptions
{
    public const string SectionName = "Notifications:WhatsApp";

    public bool Enabled { get; set; } = false;
    public string? ApiUrl { get; set; }
    public string? ApiKey { get; set; }
}
