using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Application;

public interface ITrekisRepository
{
    Task AddTrekiAsync(Treki treki);
    Task<Treki?> GetByIdAsync(TrekiId id);
    Task UpdateTrekiAsync(Treki treki);
}