using Microsoft.Extensions.Logging.EventLog;

namespace BookingsEmailCreator;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
#pragma warning disable CA1416 // Validate platform compatibility
        => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile(Path.Join("Config", "secrets.json"));
            })
            .ConfigureServices((hostContext, services) =>
            {
                try
                {
                    services.Configure<EventLogSettings>(settings =>
                    {
                        settings.SourceName = "Bookings Email Creator";
                    });
                }
                catch { /* not on windows */ }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
#pragma warning restore CA1416 // Validate platform compatibility
}
