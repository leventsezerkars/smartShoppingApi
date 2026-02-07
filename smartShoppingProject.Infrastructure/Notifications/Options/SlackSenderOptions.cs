namespace smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// Slack webhook gönderimi. appsettings: Notifications:Slack
/// </summary>
public sealed class SlackSenderOptions
{
    public const string SectionName = "Notifications:Slack";

    public bool Enabled { get; set; } = false;
    public string? DefaultWebhookUrl { get; set; }
}
