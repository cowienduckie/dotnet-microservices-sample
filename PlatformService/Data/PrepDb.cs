using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using PlatformService.Models.Platforms;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrePopulation(this IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
    }

    private static void SeedData(AppDbContext? context, bool isProduction)
    {
        if (context == null)
        {
            return;
        }

        if (isProduction)
        {
            Console.WriteLine("--> Attempting apply migrations ...");

            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data ...");

            context.Platforms.AddRange(
                new Platform { Name = ".NET 6", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}