namespace Un2Trek.Trekis.API.Controllers;

public class RefreshTokenRequest
{
    public string ExpiredAccessToken { get; set; }
    public string RefreshToken { get; set; }
}
