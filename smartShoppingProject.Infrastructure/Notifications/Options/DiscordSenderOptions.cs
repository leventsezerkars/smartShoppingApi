namespace smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// Discord webhook gönderimi. appsettings: Notifications:Discord
/// WebhookUrl burada tanımlanırsa her istekte göndermek zorunda değilsiniz.
/// </summary>
public sealed class DiscordSenderOptions
{
    public const string SectionName = "Notifications:Discord";

    public bool Enabled { get; set; } = false;
    public string? DefaultWebhookUrl { get; set; }
}
