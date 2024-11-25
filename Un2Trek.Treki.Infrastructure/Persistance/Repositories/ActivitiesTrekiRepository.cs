using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

public sealed class ActivitiesTrekiRepository : IActivitiesTrekiRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ActivitiesTrekiRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddActivityTrekiAsync(ActivityTreki activityTreki)
    {
        await _dbContext.ActivityTrekis.AddAsync(activityTreki);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ErrorOr<ActivityTreki>> GetActivityTrekiWithTrekisAsync(ActivityId activityId)
    {
        var existingActivity =  await _dbContext.ActivityTrekis
            .Include(at => at.ActivityTrekiTrekis)
            .ThenInclude(att => att.Treki)
            .FirstOrDefaultAsync(at => at.Id == activityId);
        if (existingActivity is null)
        {
            return Error.NotFound(description: "ActivityTreki not found");
        }

        return existingActivity;

    }

    public async Task<ErrorOr<Treki>> GetTrekiByIdAsync(TrekiId trekiId)
    {
        var existingTreki =  await _dbContext.Trekis
            .Include(t => t.ActivityTrekiTrekis)
            .FirstOrDefaultAsync(t => t.Id == trekiId);

        if (existingTreki is null)
        {
            return Error.NotFound(description: "Treki not found");  
        }

        return existingTreki;
    }

    public async Task<List<Treki>> GetTrekisByActivityIdAsync(ActivityId activityId)
    {
        var trekis = await (from t in _dbContext.Trekis
                            join att in _dbContext.ActivityTrekiTrekis
                            on t.Id equals att.TrekiId
                            where att.ActivityTrekiId == activityId && t.IsActive
                            select t).ToListAsync();

        return trekis;
    }

    public async Task<List<Treki>> GetTrekisNotAssignedToActivityAsync(ActivityId activityTrekiId)
    {
        var trekis = await (from t in _dbContext.Trekis
                            join att in _dbContext.ActivityTrekiTrekis
                            on t.Id equals att.TrekiId into trekisGroup
                            from att in trekisGroup.DefaultIfEmpty()
                            where att == null || att.ActivityTrekiId != activityTrekiId
                            select t).ToListAsync();

        return trekis;
    }

    public async Task<List<ActivityTreki>> GetActiveActivitiesAsync()
    {
        var currentDate = DateTime.UtcNow;
        return await _dbContext.ActivityTrekis
            .Where(a => a.ValidFromDate <= currentDate && a.ValidToDate >= currentDate)
            .ToListAsync();
    }

    public async Task<ActivityTreki?> GetByIdAsync(ActivityId activityId)
    {
        return await _dbContext.ActivityTrekis.FirstOrDefaultAsync(at => at.Id == activityId);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
