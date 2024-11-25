namespace Un2Trek.Trekis.Infrastructure;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string SecretKey { get; set; } = string.Empty;

    public int ExpirationMinutes { get; set; }

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; }
}