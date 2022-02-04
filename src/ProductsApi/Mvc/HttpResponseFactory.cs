using Microsoft.AspNetCore.Mvc;

using ProductsApi.Exceptions;

namespace ProductsApi.Mvc;

public class HttpResponseFactory
{
    public static ObjectResult UnauthorizedResponse()
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Detail = "You must provide a valid API token in Authorization header",
            Title = "Unauthorized",
            Type = "https://refactor-this/api/unauthorized",
        };

        return new(problem)
        {
            StatusCode = StatusCodes.Status401Unauthorized,
        };
    }

    public static ObjectResult UnknownErrorResponse()
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Detail = "Unexpected error occurred",
            Title = "Unexpected Error",
            Type = "https://refactor-this/api/unexpected-error",
        };

        return new(problem)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }

    public static ObjectResult TokenExpired()
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Detail = "API Token Expired",
            Title = "Your API token has expired, please ask Reliability for a new one to be assigned to you",
            Type = "https://refactor-this/api/api-token-expired",
        };

        return new(problem)
        {
            StatusCode = StatusCodes.Status401Unauthorized,
        };
    }

    public static ObjectResult WrongCredentials()
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Detail = "Could not find a user with provided credentials",
            Title = "Wrong Credentials",
            Type = "https://refactor-this/api/wrong-credentials",
        };

        return new(problem)
        {
            StatusCode = StatusCodes.Status401Unauthorized,
        };
    }

    public static ObjectResult EntityNotFoundResponse(EntityNotFoundException? exception = null)
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Detail = exception?.Message ?? "The requested entity was not found.",
            Title = "Not Found",
            Type = "https://refactor-this/api/not-found",
        };

        return new(problem)
        {
            StatusCode = StatusCodes.Status404NotFound,
        };
    }

    public static ObjectResult ModelValidationErrorsResponse(Dictionary<string, object?> errors)
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Detail = "Some request properties have invalid format",
            Title = "Validation Error",
            Type = "https://refactor-this/api/model-validation-error",
        };

        foreach (var error in errors)
        {
            problem.Extensions.Add(error);
        }

        return new(problem)
        {
            StatusCode = StatusCodes.Status400BadRequest,
        };
    }
}