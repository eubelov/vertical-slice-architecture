using RefactorThis.DataAccess.Entities;
using RefactorThis.Features.Login;

using Xunit;

namespace RefactorThis.UnitTests.Features.Login;

public class FindUserByCredentialsSpecTests : UnitTestsBaseWithInMemoryContext
{
    public FindUserByCredentialsSpecTests()
    {
        this.Context.Add(new User { Name = "Name", Password = "1234" });
        this.Context.Add(new User { Name = "user", Password = "pass" });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task CanGeUser()
    {
        await this.ReadOnlyEntityService.Single<User, FindUserByCredentialsSpec>(new("user", "pass"), AnyToken);
    }

    [Fact]
    public async Task DoesNotTrackUser()
    {
        await this.ReadOnlyEntityService.Single<User, FindUserByCredentialsSpec>(new("user", "pass"), AnyToken);
        Assert.Empty(this.Context.ChangeTracker.Entries<User>());
    }
}