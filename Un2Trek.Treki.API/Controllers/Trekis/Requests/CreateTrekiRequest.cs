using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.API.Controllers;

public class CreateTrekiRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int CaptureType { get; set; }
}