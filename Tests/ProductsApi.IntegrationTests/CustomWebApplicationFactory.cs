using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RefactorThis.IntegrationTests.Utils;

namespace RefactorThis.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("SUT");
        builder.ConfigureAppConfiguration(
            c =>
                {
                    c.AddConfiguration(
                        new ConfigurationBuilder()
                            .AddUserSecrets<CustomWebApplicationFactory<TStartup>>()
                            .Build());
                });

        builder.ConfigureServices(
            s =>
                {
                    s.AddScoped<DatabaseBuilder>();
                });
    }
}