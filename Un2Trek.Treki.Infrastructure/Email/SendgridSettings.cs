namespace Un2Trek.Trekis.Infrastructure;

public class SendgridSettings
{
    public const string SectionName = "SendgridSettings";

    public required string ApiKey { get; set; }

    public required string FromEmail { get; set; }

    public required string FromName { get; set; }

    public required string RegistrationTemplateId { get; set; }

    public required string RegistrationSubject { get; set; }
}
