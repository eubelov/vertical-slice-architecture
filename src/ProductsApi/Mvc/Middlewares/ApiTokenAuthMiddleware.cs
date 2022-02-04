using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;

using ProductsApi.Features.ValidateApiToken;

namespace ProductsApi.Mvc.Middlewares;

internal sealed class ApiTokenAuthMiddleware
{
    private readonly RequestDelegate next;

    public ApiTokenAuthMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, IMediator mediator)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        if (endpoint == null || endpoint.Metadata.Any(m => m is AllowAnonymousAttribute))
        {
            await this.next(context);
            return;
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var apiToken))
        {
            if (Guid.TryParse(apiToken, out var apiTokeGuid))
            {
                var validationResult = await mediator.Send(new ValidateApiTokenRequest { ApiToken = apiTokeGuid }, context.RequestAborted);
                if (validationResult.Result)
                {
                    await this.next(context);
                    return;
                }
            }
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(HttpResponseFactory.UnauthorizedResponse().Value);
    }
}