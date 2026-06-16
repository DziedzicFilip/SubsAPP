using Microsoft.EntityFrameworkCore;
using Backend.API.Data;
using Backend.API.Models.Configuations;

namespace Backend.API.Extensions;

public static class WebApplicationExtensions
{
    
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SubsAPP API V1");
                options.RoutePrefix = string.Empty;
            });
        }

        var httpsPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT");
        if (!string.IsNullOrWhiteSpace(httpsPort))
        {
            app.UseHttpsRedirection();
        }

        app.UseCors("DevCors");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
            Console.WriteLine("Database migrated successfully.");

            await DataSeeder.SeedAsync(services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while migrating database: {ex.Message}");
        }
    }
}
