using ErrorOr;
using System.ComponentModel.DataAnnotations;

namespace Un2Trek.Trekis.Domain;

public class ActivityTreki
{
    public ActivityId Id { get; set; }
    
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [StringLength(150)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime ValidFromDate { get; set; }

    public DateTime? ValidToDate { get; set; }

    public ICollection<ActivityTrekiTreki> ActivityTrekiTrekis { get; set; } = new List<ActivityTrekiTreki>();
    public ICollection<UserTrekiCapture> UserTrekiCaptures { get; set; } = new List<UserTrekiCapture>();


    public ErrorOr<Success> AssignTreki(Treki treki)
    {
        if (ActivityTrekiTrekis.Any(att => att.TrekiId == treki.Id))
        {
            return Error.Validation(description: "Treki is already associated with this ActivityTreki");
        }

        var activityTrekiTreki = new ActivityTrekiTreki
        {
            ActivityTrekiId = this.Id,
            TrekiId = treki.Id,
            ActivityTreki = this,
            Treki = treki
        };

        ActivityTrekiTrekis.Add(activityTrekiTreki);
        treki.ActivityTrekiTrekis.Add(activityTrekiTreki);

        return Result.Success;
    }

    public ErrorOr<Success> DesassingTreki(Treki treki)
    {
        if (ActivityTrekiTrekis is null || ActivityTrekiTrekis.Count == 0)
        {
            return Result.Success;
        }

        var activityTrekiTreki = ActivityTrekiTrekis.FirstOrDefault(att => att.TrekiId == treki.Id);
        if (activityTrekiTreki is null)
        {
            return Result.Success;
        }

        ActivityTrekiTrekis.Remove(activityTrekiTreki);

        return Result.Success;
    }
}

public class ActivityTrekiTreki
{
    public ActivityId ActivityTrekiId { get; set; }
    public ActivityTreki ActivityTreki { get; set; }
    public TrekiId TrekiId { get; set; }
    public Treki Treki { get; set; }
}
