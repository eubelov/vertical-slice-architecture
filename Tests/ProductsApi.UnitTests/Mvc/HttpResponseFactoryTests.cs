using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProductsApi.Mvc;

using Xunit;

namespace ProductsApi.UnitTests.Mvc;

public class HttpResponseFactoryTests
{
    [Fact]
    public void UnauthorizedResponseHasCorrectStatusCode()
    {
        Assert.Equal(StatusCodes.Status401Unauthorized, HttpResponseFactory.UnauthorizedResponse().StatusCode);
        Assert.Equal(StatusCodes.Status401Unauthorized, (HttpResponseFactory.UnauthorizedResponse().Value as ProblemDetails)!.Status);
    }

    [Fact]
    public void UnknownErrorResponseHasCorrectStatusCode()
    {
        Assert.Equal(StatusCodes.Status500InternalServerError, HttpResponseFactory.UnknownErrorResponse().StatusCode);
        Assert.Equal(StatusCodes.Status500InternalServerError, (HttpResponseFactory.UnknownErrorResponse().Value as ProblemDetails)!.Status);
    }

    [Fact]
    public void TokenExpiredResponseHasCorrectStatusCode()
    {
        Assert.Equal(StatusCodes.Status401Unauthorized, HttpResponseFactory.TokenExpired().StatusCode);
        Assert.Equal(StatusCodes.Status401Unauthorized, (HttpResponseFactory.TokenExpired().Value as ProblemDetails)!.Status);
    }

    [Fact]
    public void WrongCredentialsResponseHasCorrectStatusCode()
    {
        Assert.Equal(StatusCodes.Status401Unauthorized, HttpResponseFactory.WrongCredentials().StatusCode);
        Assert.Equal(StatusCodes.Status401Unauthorized, (HttpResponseFactory.WrongCredentials().Value as ProblemDetails)!.Status);
    }

    [Fact]
    public void EntityNotFoundResponseHasCorrectStatusCode()
    {
        Assert.Equal(StatusCodes.Status404NotFound, HttpResponseFactory.EntityNotFoundResponse().StatusCode);
        Assert.Equal(StatusCodes.Status404NotFound, (HttpResponseFactory.EntityNotFoundResponse().Value as ProblemDetails)!.Status);
    }

    [Fact]
    public void ModelValidationErrorsResponseHasCorrectStatusCode()
    {
        Assert.Equal(StatusCodes.Status400BadRequest, HttpResponseFactory.ModelValidationErrorsResponse(new()).StatusCode);
        Assert.Equal(
            StatusCodes.Status400BadRequest,
            (HttpResponseFactory.ModelValidationErrorsResponse(new()).Value as ProblemDetails)!.Status);
    }

    [Fact]
    public void ModelValidationErrorsResponseAddsProblemDetails()
    {
        Assert.Single((HttpResponseFactory.ModelValidationErrorsResponse(new() { ["name"] = "value" }).Value as ProblemDetails)!.Extensions);
    }
}