﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Application.Abstractions.Users;
using Un2Trek.Trekis.Infrastructure.Persistance;
using Un2Trek.Trekis.Infrastructure.Persistance.Repositories;

namespace Un2Trek.Trekis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory )
    {
        services.AddPersistence(configuration, loggerFactory);
        services.AddAuthentication(configuration);
        
        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options
            .UseSqlServer(connectionString)
            .UseLoggerFactory(loggerFactory));

        services.AddScoped<ITrekisRepository, TrekisRepository>();  
        services.AddScoped<IActivitiesTrekiRepository, ActivitiesTrekiRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
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
