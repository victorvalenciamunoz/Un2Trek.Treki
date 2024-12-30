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
        // Leer la cadena de conexión directamente de los argumentos
        string connectionString = null;

        if (args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (arg.Equals("--connection", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    connectionString = args[i + 1];
                    break;
                }
                else if (arg.StartsWith("--connection=", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = arg.Substring("--connection=".Length);
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            // Construir configuración para cargar variables de entorno y archivos JSON
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        // Configurar logging opcionalmente
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        optionsBuilder.UseLoggerFactory(loggerFactory);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}