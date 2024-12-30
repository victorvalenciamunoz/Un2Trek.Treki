namespace Un2Trek.Trekis.Infrastructure.Persistance;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Construir configuración para cargar variables de entorno y argumentos de línea de comandos
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args) // Agregar argumentos de línea de comandos
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Obtener la cadena de conexión desde argumentos o configuración
        var connectionString = configuration["connection"] // Desde argumentos (--connection)
                            ?? configuration.GetConnectionString("DefaultConnection"); // Desde archivos de configuración

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
