using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Un2Trek.Trekis.Domain;

public class ApplicationUser : IdentityUser
{
    public bool ReceivePromotionalEmails { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ErrorOr<Success> CaptureTreki(TrekiId TrekiId, ActivityId ActivityId)
    {
        var isAlreadyCaptured = UserTrekiCaptures.Any(c=> c.TrekiId == TrekiId && c.ActivityId == ActivityId);
        if (isAlreadyCaptured)
        {
            return Errors.TrekiAlreadyCaptured;
        }

        return Result.Success;
    }

    public ICollection<UserTrekiCapture> UserTrekiCaptures { get; set; } = new List<UserTrekiCapture>();
}