using MediatR;
using SendGrid;
using SendGrid.Helpers.Mail;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.API;

public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var client = new SendGridClient("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");
        var from = new EmailAddress("info@sycapps.net", "Info");
        var subject = "Confirma tu dirección de correo";
        var to = new EmailAddress(notification.Email);
        var templateId = "d-0ca4246253d348268355ae4b85ee7258"; //Un2TrekConfirmacionCorreo

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
