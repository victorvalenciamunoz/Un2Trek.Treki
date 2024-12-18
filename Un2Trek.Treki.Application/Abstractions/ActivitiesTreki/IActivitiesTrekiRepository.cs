using ErrorOr;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public interface IActivitiesTrekiRepository
{
    Task AddActivityTrekiAsync(ActivityTreki activityTreki, CancellationToken cancellationToken);
    Task<List<ActivityTreki>> GetActiveActivitiesAsync(CancellationToken cancellationToken);
    Task<ErrorOr<ActivityTreki>> GetActivityTrekiWithTrekisAsync(ActivityId activityId, CancellationToken cancellationToken);
    Task<ActivityTreki?> GetByIdAsync(ActivityId activityId, CancellationToken cancellationToken);
    Task<ErrorOr<Treki>> GetTrekiByIdAsync(TrekiId trekiId, CancellationToken cancellationToken);
    Task<List<Treki>> GetTrekisByActivityIdAsync(ActivityId activityId, CancellationToken cancellationToken);
    Task<List<Treki>> GetTrekisNotAssignedToActivityAsync(ActivityId activityTrekiId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
