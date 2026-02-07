namespace smartShoppingProject.Infrastructure.Notifications;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using smartShoppingProject.Application.Abstractions.Notifications;
using smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// Slack incoming webhook ile mesaj gönderir. WebhookUrl request'te veya config'te (DefaultWebhookUrl) olabilir.
/// </summary>
public sealed class SlackNotificationSender : INotificationSender
{
    private readonly SlackSenderOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SlackNotificationSender> _logger;

    public SlackNotificationSender(
        IOptions<SlackSenderOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<SlackNotificationSender> logger)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public NotificationChannel SupportedChannel => NotificationChannel.Slack;

    public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        var webhookUrl = request.WebhookUrl ?? _options.DefaultWebhookUrl;
        if (!_options.Enabled || string.IsNullOrWhiteSpace(webhookUrl))
        {
            _logger.LogWarning("Slack gönderimi devre dışı veya WebhookUrl yok; istek atlandı.");
            return;
        }

        var text = string.IsNullOrWhiteSpace(request.Title)
            ? request.Body
            : $"*{request.Title}*\n{request.Body}";

        var payload = new { text };
        using var client = _httpClientFactory.CreateClient();
        using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var response = await client.PostAsync(webhookUrl, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Slack webhook hatası: {StatusCode}, {Reason}", response.StatusCode, response.ReasonPhrase);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("Slack mesajı gönderildi.");
    }
}
