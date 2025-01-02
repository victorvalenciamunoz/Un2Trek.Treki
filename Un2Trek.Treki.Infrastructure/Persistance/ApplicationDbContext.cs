using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Un2Trek.Trekis.Domain;

namespace Un2Trek.Trekis.Infrastructure;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
    {}

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs())
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined.");
            }

            optionsBuilder.UseSqlServer(connectionString);
         
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        base.OnConfiguring(optionsBuilder);
    }
}
