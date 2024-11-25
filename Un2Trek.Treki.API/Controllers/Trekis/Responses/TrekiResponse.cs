namespace Un2Trek.Trekis.API.Controllers;

public class TrekiResponse
{
    public Guid ActivityId { get; init; }
    public Guid TrekiId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int CaptureType { get; init; }
    public bool IsActive { get; init; }
}

