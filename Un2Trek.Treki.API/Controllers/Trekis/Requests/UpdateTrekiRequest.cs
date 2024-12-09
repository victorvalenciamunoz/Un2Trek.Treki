namespace Un2Trek.Trekis.API.Controllers;
public class UpdateTrekiRequest
{
    public Guid Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int CaptureType { get; set; }
}

