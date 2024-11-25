using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Treki> Trekis => Set<Treki>();

    public DbSet<ActivityTreki> ActivityTrekis => Set<ActivityTreki>();

    public DbSet<ActivityTrekiTreki> ActivityTrekiTrekis => Set<ActivityTrekiTreki>();

    public DbSet<UserTrekiCapture> UserTrekiCaptures => Set<UserTrekiCapture>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
