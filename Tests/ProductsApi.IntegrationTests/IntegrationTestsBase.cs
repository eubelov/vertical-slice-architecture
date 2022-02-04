using System.Net;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using ProductsApi.DataAccess;
using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests;

public abstract class IntegrationTestsBase : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    protected IntegrationTestsBase(CustomWebApplicationFactory<Startup> factory)
    {
        this.Factory = factory;
        var serviceProvider = factory.Services.GetService<IServiceProvider>()!;
        this.CreateContext = () => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Context>();
        this.DatabaseBuilder = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DatabaseBuilder>();
    }

    protected CustomWebApplicationFactory<Startup> Factory { get; }

    protected Func<Context> CreateContext { get; }

    protected DatabaseBuilder DatabaseBuilder { get; }

    protected WebApplicationFactory<Startup> CreateApplicationFactory(Action<IServiceCollection>? customConfig = null)
    {
        return this.Factory.WithWebHostBuilder(
            builder =>
                {
                    builder.ConfigureServices(
                        s => { customConfig?.Invoke(s); });
                });
    }

    protected HttpClient CreateClient(Guid? apiToken = null)
    {
        var client = this.CreateApplicationFactory().CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", apiToken?.ToString() ?? Constants.UserToken.ToString());

        return client;
    }

    protected Task<HttpCallResult<T>> Delete<T>(string route, Guid? apiToken = null)
    {
        var client = this.CreateClient(apiToken);

        return this.Delete<T>(client, route);
    }

    protected async Task<HttpCallResult<T>> Delete<T>(HttpClient client, string route)
    {
        var result = await client.DeleteAsync(route);
        var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

        return new(response, result.StatusCode)
        {
            Headers = result.Headers,
        };
    }

    protected Task<HttpCallResult<T>> Get<T>(string route, Guid? apiToken = null)
    {
        var client = this.CreateClient(apiToken);

        return this.Get<T>(client, route);
    }

    protected async Task<HttpCallResult<T>> Get<T>(HttpClient client, string route)
    {
        var result = await client.GetAsync(route);
        var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

        return new(response, result.StatusCode)
        {
            Headers = result.Headers,
        };
    }

    protected Task<HttpCallResult<T>> Post<T>(string route, object payload, Guid? apiToken = null)
    {
        var client = this.CreateClient(apiToken);

        return this.Post<T>(client, route, payload);
    }

    protected async Task<HttpCallResult<T>> Post<T>(HttpClient client, string route, object payload)
    {
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var result = await client.PostAsync(route, content);
        var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

        return new(response, result.StatusCode)
        {
            Headers = result.Headers,
        };
    }

    protected Task<HttpCallResult<T>> Put<T>(string route, object payload, Guid? apiToken = null)
    {
        var client = this.CreateClient(apiToken);

        return this.Put<T>(client, route, payload);
    }

    protected async Task<HttpCallResult<T>> Put<T>(HttpClient client, string route, object payload)
    {
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var result = await client.PutAsync(route, content);
        var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

        return new(response, result.StatusCode)
        {
            Headers = result.Headers,
        };
    }

    public record HttpCallResult<T>(T? Data, HttpStatusCode StatusCode)
    {
        public HttpResponseHeaders? Headers { init; get; }
    }
}