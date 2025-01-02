using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Un2Trek.Trekis.API;
using Un2Trek.Trekis.Application;
using Un2Trek.Trekis.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();
builder.Host.UseSerilog();

var loggerFactory = LoggerFactory.Create(loggingBuilder => { loggingBuilder.AddSerilog(); });
builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, loggerFactory);
builder.Services.AddApiServices(builder.Configuration);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using (var serviceScope = app.Services.CreateAsyncScope())
    {
       await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", name: "v1");
    });
}

app.UseHttpsRedirection();

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();