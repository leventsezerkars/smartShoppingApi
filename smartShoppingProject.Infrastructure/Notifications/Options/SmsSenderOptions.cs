namespace smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// SMS gönderimi için yapılandırma. appsettings: Notifications:Sms
/// Twilio vb. entegrasyonunda AccountSid, AuthToken, FromNumber kullanılabilir.
/// </summary>
public sealed class SmsSenderOptions
{
    public const string SectionName = "Notifications:Sms";

    public bool Enabled { get; set; } = false;
    public string? AccountSid { get; set; }
    public string? AuthToken { get; set; }
    public string? FromNumber { get; set; }
}
