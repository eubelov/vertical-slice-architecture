using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ProductsApi.Features.Login;
using ProductsApi.Mvc;

namespace ProductsApi.Endpoints.Auth;

public sealed class Login : EndpointBase
{
    /// <summary>
    /// Logs user in.
    /// </summary>
    /// <response code="200">Log in succeeded.</response>
    /// <response code="400">Some of the request's parameters have invalid format.</response>
    /// <response code="401">Either the user name or password is wrong, or API token expired. Examine the response for details. </response>
    /// <response code="500">Unexpected error occured. Examine the response for details. Examine the response for details. </response>
    [HttpPost("auth/login", Name = "Login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [ApiExplorerSettings(GroupName = "Auth")]
    public async Task<IActionResult> Execute([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        IActionResult ToHttpResponse(LoginResponse response) => response.FailureReasonValue switch
        {
            LoginResponse.FailureReason.ExpiredToken => HttpResponseFactory.TokenExpired(),
            LoginResponse.FailureReason.WrongUserNameOrPassword => HttpResponseFactory.WrongCredentials(),
            _ => this.Ok(new LoginResult { Token = response.Token!.Value }),
        };

        return await this.Send(loginRequest, ToHttpResponse, cancellationToken);
    }

    private class LoginResult
    {
        public Guid Token { get; init; }
    }
}