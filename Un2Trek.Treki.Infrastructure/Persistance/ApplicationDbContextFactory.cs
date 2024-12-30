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
        // Construir configuración para cargar variables de entorno, argumentos de línea de comandos y User Secrets
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables() // Agregar variables de entorno
            .AddUserSecrets(Assembly.Load("Un2Trek.Trekis.API")); // Agregar User Secrets

        if (args != null && args.Length > 0)
        {
            // Agregar argumentos de línea de comandos a la configuración
            configurationBuilder.AddCommandLine(args);
        }

        // Cargar archivos de configuración si existen
        if (File.Exists("appsettings.json"))
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrEmpty(environment) && File.Exists($"appsettings.{environment}.json"))
        {
            configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true);
        }

        var configuration = configurationBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Obtener la cadena de conexión desde argumentos, variables de entorno o configuración
        var connectionString = configuration["connection"] // Desde argumentos (--connection)
                            ?? configuration.GetConnectionString("DefaultConnection") // Desde archivos de configuración
                            ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"); // Desde variables de entorno

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

