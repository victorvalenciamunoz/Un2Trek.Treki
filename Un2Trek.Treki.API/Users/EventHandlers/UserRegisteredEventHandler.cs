using MediatR;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Un2Trek.Trekis.Domain;
using Un2Trek.Trekis.Infrastructure;

namespace Un2Trek.Trekis.API;

public class UserRegisteredEventHandler(IOptions<SendgridSettings> sendgridSettings) : INotificationHandler<UserRegisteredEvent>
{    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var client = new SendGridClient(sendgridSettings.Value.ApiKey);
        var from = new EmailAddress(sendgridSettings.Value.FromEmail,sendgridSettings.Value.FromName);
        var subject = sendgridSettings.Value.RegistrationSubject;
        var to = new EmailAddress(notification.Email);
        var templateId = sendgridSettings.Value.RegistrationTemplateId; //Un2TrekConfirmacionCorreo

        var msg = new SendGridMessage
        {
            From = from,
            TemplateId = templateId,
            Subject = subject
        };

        msg.AddTo(to);
        msg.SetTemplateData(new
        {
            confirm_url = notification.ConfirmationLink
        });

        var response = await client.SendEmailAsync(msg);
    }
}
