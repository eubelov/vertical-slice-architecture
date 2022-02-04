using ProductsApi.Features.ValidateApiToken;

using Xunit;

namespace ProductsApi.UnitTests.Features.ValidateApiToken;

public class ValidateApiTokenRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid apiToken = Guid.NewGuid();

    private readonly ValidateApiTokenRequestHandler handler;

    public ValidateApiTokenRequestHandlerTests()
    {
        this.Context.Users.Add(
            new()
            {
                ApiToken = this.apiToken.ToString(),
                ApiTokenExpiry = this.DateTimeProvider.Object.Now.AddMinutes(1).ToUnixTimeMilliseconds(),
            });

        this.Context.SaveChanges();

        this.handler = this.Mocker.CreateInstance<ValidateApiTokenRequestHandler>();
    }

    [Fact]
    public async Task ReturnsFalseIfTokenDoesNotExist()
    {
        var result = await this.handler.Handle(new() { ApiToken = Guid.NewGuid() }, AnyToken);

        Assert.False(result.Result);
    }

    [Fact]
    public async Task ReturnsFalseIfTokenExpired()
    {
        this.DateTimeProvider.SetupGet(x => x.Now).Returns(DateTime.Parse("2021-01-01T16:00:00"));

        var result = await this.handler.Handle(new() { ApiToken = Guid.NewGuid() }, AnyToken);

        Assert.False(result.Result);
    }

    [Fact]
    public async Task ReturnsTrueIfTokenExistAndNotExpired()
    {
        var result = await this.handler.Handle(new() { ApiToken = this.apiToken }, AnyToken);

        Assert.True(result.Result);
    }
}