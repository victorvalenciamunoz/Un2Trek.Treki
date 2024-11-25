using Microsoft.EntityFrameworkCore;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure.Persistance;

public sealed class TrekisRepository : ITrekisRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TrekisRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Treki?> GetByIdAsync(TrekiId id)
    {
        return await _dbContext.Trekis.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddTrekiAsync(Treki treki)
    {
        await _dbContext.Trekis.AddAsync(treki);
        await _dbContext.SaveChangesAsync();
    }
}
