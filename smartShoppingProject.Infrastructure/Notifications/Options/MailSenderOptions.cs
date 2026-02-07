namespace smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// SMTP ile e-posta gönderimi için yapılandırma. appsettings: Notifications:Mail
/// </summary>
public sealed class MailSenderOptions
{
    public const string SectionName = "Notifications:Mail";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string FromAddress { get; set; } = "noreply@localhost";
    public string? FromName { get; set; }
    public bool Enabled { get; set; } = true;
}
