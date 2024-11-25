using ErrorOr;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public interface IActivitiesTrekiRepository
{
    Task AddActivityTrekiAsync(ActivityTreki activityTreki);
    Task<List<ActivityTreki>> GetActiveActivitiesAsync();
    Task<ErrorOr<ActivityTreki>> GetActivityTrekiWithTrekisAsync(ActivityId activityId);
    Task<ActivityTreki?> GetByIdAsync(ActivityId activityId);
    Task<ErrorOr<Treki>> GetTrekiByIdAsync(TrekiId trekiId);
    Task<List<Treki>> GetTrekisByActivityIdAsync(ActivityId activityId);
    Task<List<Treki>> GetTrekisNotAssignedToActivityAsync(ActivityId activityTrekiId);
    Task SaveChangesAsync();
}
