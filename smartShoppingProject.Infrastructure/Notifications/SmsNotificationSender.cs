namespace smartShoppingProject.Infrastructure.Notifications;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using smartShoppingProject.Application.Abstractions.Notifications;
using smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// SMS gönderimi. Şu an yapılandırma ve log ile stub; Twilio vb. entegrasyonu eklenebilir.
/// </summary>
public sealed class SmsNotificationSender : INotificationSender
{
    private readonly SmsSenderOptions _options;
    private readonly ILogger<SmsNotificationSender> _logger;

    public SmsNotificationSender(IOptions<SmsSenderOptions> options, ILogger<SmsNotificationSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public NotificationChannel SupportedChannel => NotificationChannel.Sms;

    public Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogWarning("SMS gönderimi devre dışı; istek atlandı.");
            return Task.CompletedTask;
        }

        var phone = request.PhoneNumber ?? "(numara yok)";
        _logger.LogInformation("SMS (stub): To={Phone}, Message={Message}", phone, request.Body);

        // TODO: Twilio veya başka bir SMS sağlayıcı entegrasyonu:
        // await _twilioClient.Messages.CreateAsync(new CreateMessageOptions(...));

        return Task.CompletedTask;
    }
}
