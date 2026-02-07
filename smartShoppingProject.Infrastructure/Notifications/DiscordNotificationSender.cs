namespace smartShoppingProject.Infrastructure.Notifications;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using smartShoppingProject.Application.Abstractions.Notifications;
using smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// Discord webhook ile mesaj gönderir. WebhookUrl request'te veya config'te (DefaultWebhookUrl) olabilir.
/// </summary>
public sealed class DiscordNotificationSender : INotificationSender
{
    private readonly DiscordSenderOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DiscordNotificationSender> _logger;

    public DiscordNotificationSender(
        IOptions<DiscordSenderOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<DiscordNotificationSender> logger)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public NotificationChannel SupportedChannel => NotificationChannel.Discord;

    public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        var webhookUrl = request.WebhookUrl ?? _options.DefaultWebhookUrl;
        if (!_options.Enabled || string.IsNullOrWhiteSpace(webhookUrl))
        {
            _logger.LogWarning("Discord gönderimi devre dışı veya WebhookUrl yok; istek atlandı.");
            return;
        }

        var content = string.IsNullOrWhiteSpace(request.Title)
            ? request.Body
            : $"**{request.Title}**\n{request.Body}";

        var payload = new { content };
        using var client = _httpClientFactory.CreateClient();
        using var response = await client.PostAsJsonAsync(webhookUrl, payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Discord webhook hatası: {StatusCode}, {Reason}", response.StatusCode, response.ReasonPhrase);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("Discord mesajı gönderildi: WebhookUrl (masked)");
    }
}
