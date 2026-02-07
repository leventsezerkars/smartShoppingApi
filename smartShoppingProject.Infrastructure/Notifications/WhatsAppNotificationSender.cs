namespace smartShoppingProject.Infrastructure.Notifications;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using smartShoppingProject.Application.Abstractions.Notifications;
using smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// WhatsApp mesajı gönderir. ApiUrl ile tanımlı bir gateway'e POST atar (to + message).
/// ApiUrl yoksa stub olarak log atar; Twilio vb. entegrasyonu ApiUrl + ApiKey ile bağlanabilir.
/// </summary>
public sealed class WhatsAppNotificationSender : INotificationSender
{
    private readonly WhatsAppSenderOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WhatsAppNotificationSender> _logger;

    public WhatsAppNotificationSender(
        IOptions<WhatsAppSenderOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<WhatsAppNotificationSender> logger)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public NotificationChannel SupportedChannel => NotificationChannel.WhatsApp;

    public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        var phone = request.PhoneNumber?.Trim();
        if (string.IsNullOrEmpty(phone))
        {
            _logger.LogWarning("WhatsApp alıcı (PhoneNumber) boş; gönderim atlandı.");
            return;
        }

        if (!_options.Enabled)
        {
            _logger.LogWarning("WhatsApp gönderimi devre dışı; istek atlandı.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_options.ApiUrl))
        {
            _logger.LogInformation("WhatsApp (stub): To={Phone}, Message={Message}", phone, request.Body);
            return;
        }

        var payload = new { to = phone, message = request.Body };
        using var client = _httpClientFactory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.ApiUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
            requestMessage.Headers.TryAddWithoutValidation("Authorization", _options.ApiKey);

        using var response = await client.SendAsync(requestMessage, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("WhatsApp API hatası: {StatusCode}, {Reason}", response.StatusCode, response.ReasonPhrase);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("WhatsApp mesajı gönderildi: To={Phone}", phone);
    }
}
