using Serilog;

namespace ProductsApi;

public class Program
{
    public static IHostBuilder CreateWebHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .UseSerilog(Mvc.Extensions.LoggingExtensions.ConfigureSerilog)
        .ConfigureAppConfiguration(
            (hostingContext, builder) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                        .AddEnvironmentVariables();

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<Program>();
                    }
                })
        .ConfigureWebHostDefaults(
            webHostBuilder =>
                {
                    webHostBuilder.UseStartup<Startup>();
                });

    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }
}