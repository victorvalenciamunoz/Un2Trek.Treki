namespace Un2Trek.Trekis.API.Controllers;

public class CaptureTrekiRequest
{
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }

    public string ActivityId { get; set; }
    
    public string TrekiId { get; set; }

    public string UserId { get; set; }
}
