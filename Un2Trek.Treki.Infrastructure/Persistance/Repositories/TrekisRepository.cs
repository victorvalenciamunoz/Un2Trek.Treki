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

    public async Task<Treki?> GetByIdAsync(TrekiId id, CancellationToken cancellationToken)
    {
        return await _dbContext.Trekis.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task AddTrekiAsync(Treki treki, CancellationToken cancellationToken)
    {
        _dbContext.Trekis.Add(treki);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTrekiAsync(Treki treki, CancellationToken cancellationToken)
    {
        _dbContext.Trekis.Update(treki);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
