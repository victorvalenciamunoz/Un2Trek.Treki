namespace Un2Trek.Trekis.API.Controllers;

public class AssignTrekiToActivityRequest
{
    public Guid TrekiId { get; set; }
    public Guid ActivityId { get; set; }
}
