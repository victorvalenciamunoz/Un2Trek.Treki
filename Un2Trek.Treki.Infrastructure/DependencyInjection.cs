using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Infrastructure.Persistance;

namespace Un2Trek.Trekis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddAuthentication(configuration);
        
        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ITrekisRepository, TrekisRepository>();  
        services.AddScoped<IActivitiesTrekiRepository, ActivitiesTrekiRepository>();
        services.AddScoped<ICaptureTrekisRepository, CaptureTrekisRepository>();

        return services;
    }
    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));        

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

}
