using CommandService.Models;
using CommandService.Models.Platforms;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
        var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();

        if (grpcClient == null || repo == null)
        {
            return;
        }

        var platforms = grpcClient.ReturnAllPlatforms();

        SeedData(repo, platforms);
    }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding new Platforms...");

        foreach (var platform in platforms)
        {
            if (!repo.ExternalPlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
            }

            repo.SaveChanges();
        }
    }
}