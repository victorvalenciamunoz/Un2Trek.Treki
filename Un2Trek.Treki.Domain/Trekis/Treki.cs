using System.ComponentModel.DataAnnotations;
using Un2Trek.Trekis.Domain.ValueObjects;

namespace Un2Trek.Trekis.Domain;


public class Treki
{
    public TrekiId Id { get; init; }

    public Location Location { get; init; }

    [StringLength(50)]
    public string Title { get; init; } = string.Empty;

    [StringLength(250)]
    public string Description { get; init; } = string.Empty;

    public CaptureType CaptureType { get; init; }

    public bool IsActive { get; init; }

    public ICollection<ActivityTrekiTreki> ActivityTrekiTrekis { get; set; } = new List<ActivityTrekiTreki>();

    public ICollection<UserTrekiCapture> UserTrekiCaptures { get; set; } = new List<UserTrekiCapture>();
    public Treki()
    {
        
    }

    public Treki(Location location, string title, string description, bool isActive, CaptureType captureType )
    {
        Id = TrekiId.From(Guid.NewGuid());
        Location = location;
        Title = title;
        Description = description;
        IsActive = isActive;
        CaptureType = captureType;
    }
}