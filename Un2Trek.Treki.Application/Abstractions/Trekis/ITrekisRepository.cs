using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public interface ITrekisRepository
{
    Task AddTrekiAsync(Treki treki, CancellationToken cancellationToken);
    Task<Treki?> GetByIdAsync(TrekiId id, CancellationToken cancellationToken);
    Task UpdateTrekiAsync(Treki treki, CancellationToken cancellationToken);
}