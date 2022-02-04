using RefactorThis.DataAccess.Entities;
using RefactorThis.Features.ValidateApiToken;

using Xunit;

namespace RefactorThis.UnitTests.Features.ValidateApiToken;

public class FindUserByApiTokenSpecTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid apiToken = Guid.NewGuid();

    public FindUserByApiTokenSpecTests()
    {
        this.Context.Add(
            new User
            {
                Name = "Name",
                Password = "1234",
                ApiToken = this.apiToken.ToString(),
                ApiTokenExpiry = this.DateTimeProvider.Object.Now.AddDays(1).ToUnixTimeMilliseconds(),
            });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task CanGeUserByApiToken()
    {
        var expiry = this.DateTimeProvider.Object.Now.ToUnixTimeMilliseconds();
        Assert.True(await this.ReadOnlyEntityService.Exists(new FindUserByApiTokenSpec(this.apiToken, expiry), AnyToken));
    }

    [Fact]
    public async Task DoesNotTrackUser()
    {
        var expiry = this.DateTimeProvider.Object.Now.ToUnixTimeMilliseconds();
        await this.ReadOnlyEntityService.Exists(new FindUserByApiTokenSpec(this.apiToken, expiry), AnyToken);

        Assert.Empty(this.Context.ChangeTracker.Entries<User>());
    }
}