using System.ComponentModel.DataAnnotations;
using Un2Trek.Trekis.Domain.ValueObjects;

namespace Un2Trek.Trekis.Domain;


public class Treki
{
    public TrekiId Id { get; private set; }

    public Location Location { get; private set; }

    [StringLength(50)]
    public string Title { get; private set; } = string.Empty;

    [StringLength(250)]
    public string Description { get; private set; } = string.Empty;

    public CaptureType CaptureType { get; private set; }

    public bool IsActive { get; private set; }

    public ICollection<ActivityTrekiTreki> ActivityTrekiTrekis { get; set; } = new List<ActivityTrekiTreki>();

    public ICollection<UserTrekiCapture> UserTrekiCaptures { get; set; } = new List<UserTrekiCapture>();
    public Treki()
    {

    }

    public Treki(Location location, string title, string description, bool isActive, CaptureType captureType)
    {
        Id = TrekiId.From(Guid.NewGuid());
        Location = location;
        Title = title;
        Description = description;
        IsActive = isActive;
        CaptureType = captureType;
    }    

    public void SetProperties(TrekiId id, Location location, string title, string description, bool isActive, CaptureType captureType)
    {
        Id = id;
        Location = location;
        Title = title;
        Description = description;
        IsActive = isActive;
        CaptureType = captureType; 
    }
}