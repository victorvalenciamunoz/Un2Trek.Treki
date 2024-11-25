using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

public sealed class CaptureTrekisRepository : ICaptureTrekisRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CaptureTrekisRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CaptureTrekiAsync(UserTrekiCapture capture)
    {
        await _dbContext.UserTrekiCaptures.AddAsync(capture);
        await _dbContext.SaveChangesAsync();
    }
}