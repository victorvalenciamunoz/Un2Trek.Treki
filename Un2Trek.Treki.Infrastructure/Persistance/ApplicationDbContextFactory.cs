namespace Un2Trek.Trekis.Infrastructure.Persistance;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Construir configuración para cargar variables de entorno y User Secrets
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Agregar appsettings.json
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true) // Agregar appsettings.{Environment}.json
            .AddEnvironmentVariables() // Agregar variables de entorno
            .AddUserSecrets(Assembly.Load("Un2Trek.Trekis.API"))
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Obtener la cadena de conexión desde variables de entorno o User Secrets
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined.");
        }

        optionsBuilder.UseSqlServer(connectionString);

        // Configurar logging opcionalmente
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        optionsBuilder.UseLoggerFactory(loggerFactory);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

