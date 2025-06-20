using Project_PRN222_G5.DataAccess.Data.Seeder;

namespace Project_PRN222_G5.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Read Configuration from appsettings
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

        var host = CreateHostBuilder(args).Build();

        // Get logger
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            // Seed data
            await DatabaseSeeder.SeedAsync(scope.ServiceProvider);

            logger.LogInformation("Running application...");
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Application failed to start.");
            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}