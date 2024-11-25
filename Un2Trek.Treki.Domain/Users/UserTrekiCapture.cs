namespace Un2Trek.Trekis.Domain;

public class UserTrekiCapture
{
    public string UserId { get; private set; }

    public TrekiId TrekiId { get; private set; }

    public ActivityId ActivityId { get; private set; }

    public DateTime CaptureDate { get; private set; }


    public ApplicationUser User { get; private set; }
    public Treki Treki { get; private set; }
    public ActivityTreki ActivityTreki { get; private set; }

    public UserTrekiCapture(string userId, TrekiId trekiId, ActivityId activityId, DateTime captureDate)
    {
        UserId = userId;
        TrekiId = trekiId;
        CaptureDate = captureDate;
        ActivityId = activityId;
    }
}