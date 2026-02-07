namespace smartShoppingProject.Infrastructure.Notifications;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using smartShoppingProject.Application.Abstractions.Notifications;
using smartShoppingProject.Infrastructure.Notifications.Options;

/// <summary>
/// SMTP ile e-posta gönderir. MailKit kullanır; yapılandırma Notifications:Mail bölümünden okunur.
/// </summary>
public sealed class MailNotificationSender : INotificationSender
{
    private readonly MailSenderOptions _options;
    private readonly ILogger<MailNotificationSender> _logger;

    public MailNotificationSender(IOptions<MailSenderOptions> options, ILogger<MailNotificationSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public NotificationChannel SupportedChannel => NotificationChannel.Email;

    public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogWarning("Mail gönderimi devre dışı; istek atlandı.");
            return;
        }

        if (string.IsNullOrWhiteSpace(request.To))
        {
            _logger.LogWarning("E-posta alıcı (To) boş; gönderim atlandı.");
            return;
        }

        var subject = request.Subject ?? "(Konu yok)";
        var body = request.HtmlBody ?? request.Body;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName ?? _options.FromAddress, _options.FromAddress));
        message.To.Add(MailboxAddress.Parse(request.To));
        message.Subject = subject;
        message.Body = new TextPart(string.IsNullOrEmpty(request.HtmlBody) ? TextFormat.Plain : TextFormat.Html)
        {
            Text = body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_options.UserName))
            await client.AuthenticateAsync(_options.UserName, _options.Password, cancellationToken);

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        _logger.LogInformation("E-posta gönderildi: To={To}, Subject={Subject}", request.To, subject);
    }
}
