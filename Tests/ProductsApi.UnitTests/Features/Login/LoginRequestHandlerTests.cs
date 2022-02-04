using RefactorThis.Features.Login;
using RefactorThis.Metrics;

using Xunit;

namespace RefactorThis.UnitTests.Features.Login;

public class LoginRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid apiToken = Guid.NewGuid();

    private readonly LoginRequestHandler handler;

    public LoginRequestHandlerTests()
    {
        this.Context.Users.Add(
            new()
            {
                Name = "test",
                Password = "1234",
                ApiToken = this.apiToken.ToString(),
                ApiTokenExpiry = this.DateTimeProvider.Object.Now.AddMinutes(1).ToUnixTimeMilliseconds(),
            });
        this.Context.SaveChanges();

        this.UseNullLoggerFor<LoginRequestHandler>();

        this.handler = this.Mocker.CreateInstance<LoginRequestHandler>();
    }

    [Fact]
    public async Task ReturnsWrongUserNameOrPasswordIfUserNotFound()
    {
        var result = await this.handler.Handle(new() { Password = "pass", UserName = "user" }, AnyToken);

        Assert.NotNull(result.Result);
        Assert.Equal(LoginResponse.FailureReason.WrongUserNameOrPassword, result.Result.FailureReasonValue);
    }

    [Fact]
    public async Task ReturnNullTokenIfUserNotFound()
    {
        var result = await this.handler.Handle(new() { Password = "pass", UserName = "user" }, AnyToken);

        Assert.Null(result.Result!.Token);
    }

    [Fact]
    public async Task IncrementsCounterIfUserNotFound()
    {
        await this.handler.Handle(new() { Password = "pass", UserName = "user" }, AnyToken);

        Assert.True(ApiMetrics.FailedLoginAttemptsCount.Labels(nameof(LoginResponse.FailureReason.WrongUserNameOrPassword)).Value > 0);
    }

    [Fact]
    public async Task ReturnsTokenExpiredWhenTokenExpired()
    {
        this.DateTimeProvider.SetupGet(x => x.Now).Returns(DateTime.Parse("2022-01-01T16:00:00"));

        var result = await this.handler.Handle(new() { Password = "1234", UserName = "test" }, AnyToken);

        Assert.NotNull(result.Result);
        Assert.Equal(LoginResponse.FailureReason.ExpiredToken, result.Result.FailureReasonValue);
    }

    [Fact]
    public async Task IncrementsCounterIfTokenExpired()
    {
        this.DateTimeProvider.SetupGet(x => x.Now).Returns(DateTime.Parse("2022-01-01T16:00:00"));

        await this.handler.Handle(new() { Password = "1234", UserName = "test" }, AnyToken);

        Assert.True(ApiMetrics.FailedLoginAttemptsCount.Labels(nameof(LoginResponse.FailureReason.ExpiredToken)).Value > 0);
    }

    [Fact]
    public async Task ReturnTokenIfNotExpired()
    {
        var result = await this.handler.Handle(new() { Password = "1234", UserName = "test" }, AnyToken);

        Assert.Equal(this.apiToken, result.Result!.Token);
    }
}