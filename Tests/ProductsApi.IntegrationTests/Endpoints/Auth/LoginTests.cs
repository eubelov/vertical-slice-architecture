using System.Net;

using Microsoft.AspNetCore.Mvc;

using RefactorThis.Features.Login;
using RefactorThis.IntegrationTests.Utils;
using RefactorThis.Metrics;

using Xunit;

namespace RefactorThis.IntegrationTests.Endpoints.Auth;

public class LoginTests : IntegrationTestsBase
{
    public LoginTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task ReturnsApiTokenForExistingUser()
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .Build();

        var loginModel = new LoginRequest
        {
            Password = Constants.Password,
            UserName = Constants.UserName,
        };

        var (result, statusCode) = await this.Post<LoginResult>(Constants.Routes.Auth.Login, loginModel);

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal(Constants.UserToken, result!.Token);
    }

    [Fact]
    public async Task Returns400WhenLoginModelIsInvalid()
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .Build();

        var loginModel = new LoginRequest
        {
            Password = new('x', 10),
            UserName = Constants.UserName,
        };

        var (result, statusCode) = await this.Post<ProblemDetails>(Constants.Routes.Auth.Login, loginModel);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns401ForNonexistentUser()
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .Build();

        var loginModel = new LoginRequest
        {
            Password = "pwd",
            UserName = "user32",
        };

        var (result, statusCode) = await this.Post<ProblemDetails>(Constants.Routes.Auth.Login, loginModel);

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.WrongCredentials, result!.Type);

        Assert.Equal(1, ApiMetrics.FailedLoginAttemptsCount.Labels(nameof(LoginResponse.FailureReason.WrongUserNameOrPassword)).Value);
    }

    [Fact]
    public async Task Returns401IfTokenExpired()
    {
        this.DatabaseBuilder.Initialize()
            .WithUser(modifier: user => user.ApiTokenExpiry = DateTimeOffset.Now.AddDays(-1).ToUnixTimeMilliseconds())
            .Build();

        var loginModel = new LoginRequest
        {
            Password = Constants.Password,
            UserName = Constants.UserName,
        };

        var (result, statusCode) = await this.Post<ProblemDetails>(Constants.Routes.Auth.Login, loginModel);

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.TokenExpired, result!.Type);

        Assert.Equal(1, ApiMetrics.FailedLoginAttemptsCount.Labels(nameof(LoginResponse.FailureReason.ExpiredToken)).Value);
    }

    private sealed class LoginRequest
    {
        public string UserName { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;
    }

    private class LoginResult
    {
        public Guid? Token { get; init; }
    }
}