using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Moq;

using ProductsApi.Features.ValidateApiToken;
using ProductsApi.Mvc.Middlewares;

using Xunit;

namespace ProductsApi.UnitTests.Mvc.Middlewares;

public class ApiTokenAuthMiddlewareTests : UnitTestBase
{
    private readonly Guid apiToken = Guid.NewGuid();

    public ApiTokenAuthMiddlewareTests()
    {
        var endpoint = this.Mocker.GetMock<IEndpointFeature>();
        endpoint
            .Setup(x => x.Endpoint)
            .Returns(new Endpoint(x => x.Response.WriteAsJsonAsync(new { }), EndpointMetadataCollection.Empty, "test"));

        this.Mediator
            .Setup(x => x.Send(It.IsAny<ValidateApiTokenRequest>(), AnyToken))
            .ReturnsAsync((ValidateApiTokenRequest r, CancellationToken _) => new() { Result = r.ApiToken == this.apiToken });
    }

    [Fact]
    public async Task Returns200TokenIsValid()
    {
        var context = this.CreateContext(true);
        context.Request.Headers.Add("Authorization", this.apiToken.ToString());
        var middlewareInstance = new ApiTokenAuthMiddleware(c => c.Response.WriteAsJsonAsync(new { }));
        await middlewareInstance.InvokeAsync(context, this.Mediator.Object);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);

        this.Mediator
            .Verify(x => x.Send(It.IsAny<ValidateApiTokenRequest>(), AnyToken), Times.Once);

        this.Mediator.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Returns401IfTokenHasInvalidFormat()
    {
        var context = this.CreateContext(true);
        context.Request.Headers.Add("Authorization", "11234");
        var middlewareInstance = new ApiTokenAuthMiddleware(c => c.Response.WriteAsJsonAsync(new { }));
        await middlewareInstance.InvokeAsync(context, this.Mediator.Object);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

        this.Mediator.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Returns401IfTokenNotPresentInAuthHeader()
    {
        var context = this.CreateContext(true);
        var middlewareInstance = new ApiTokenAuthMiddleware(c => c.Response.WriteAsJsonAsync(new { }));
        await middlewareInstance.InvokeAsync(context, this.Mediator.Object);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

        this.Mediator.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Returns401IfUnknownToken()
    {
        var context = this.CreateContext(true);
        context.Request.Headers.Add("Authorization", Guid.NewGuid().ToString());
        var middlewareInstance = new ApiTokenAuthMiddleware(c => c.Response.WriteAsJsonAsync(new { }));
        await middlewareInstance.InvokeAsync(context, this.Mediator.Object);

        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

        this.Mediator
            .Verify(x => x.Send(It.IsAny<ValidateApiTokenRequest>(), AnyToken), Times.Once);

        this.Mediator.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SkipsApiTokenValidationIfEndpointFeatureIsOff()
    {
        var context = this.CreateContext(false);
        var middlewareInstance = new ApiTokenAuthMiddleware(c => c.Response.WriteAsJsonAsync(new { }));
        await middlewareInstance.InvokeAsync(context, this.Mediator.Object);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);

        this.Mediator.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SkipsApiTokenValidationIfEndpoinHasAllowAnonymousAttribute()
    {
        this.Mocker.GetMock<IEndpointFeature>()
            .Setup(x => x.Endpoint)
            .Returns(new Endpoint(x => x.Response.WriteAsJsonAsync(new { }), new(new AllowAnonymousAttribute()), "test"));

        var context = this.CreateContext(false);
        var middlewareInstance = new ApiTokenAuthMiddleware(c => c.Response.WriteAsJsonAsync(new { }));
        await middlewareInstance.InvokeAsync(context, this.Mediator.Object);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);

        this.Mediator.VerifyNoOtherCalls();
    }

    private DefaultHttpContext CreateContext(bool withEndpointFeature)
    {
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream(),
            },
            Request =
            {
                Path = "/",
            },
        };

        if (withEndpointFeature)
        {
            context.Features.Set(this.Mocker.GetMock<IEndpointFeature>().Object);
        }

        return context;
    }
}