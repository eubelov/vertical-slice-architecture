using RefactorThis.DataAccess.Entities;
using RefactorThis.Features.FindProducts;

using Xunit;

namespace RefactorThis.UnitTests.Features.FindProducts;

public class FindProductsByNameSpecTests : UnitTestsBaseWithInMemoryContext
{
    private const string SearchString = "pr42";

    public FindProductsByNameSpecTests()
    {
        this.Context.Add(new Product { Id = Guid.NewGuid(), Name = SearchString + "Name" });
        this.Context.Add(new Product { Id = Guid.NewGuid(), Name = "a" + SearchString + "Name" });
        this.Context.Add(new Product { Id = Guid.NewGuid(), Name = "Name" });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task CanFindProductsByName()
    {
        var products = await this.ReadOnlyEntityService.LoadAll(new FindProductsByNameSpec(SearchString), AnyToken);
        Assert.Equal(2, products.Length);
    }

    [Fact]
    public async Task DoesNotTrackProducts()
    {
        await this.ReadOnlyEntityService.LoadAll(new FindProductsByNameSpec(SearchString), AnyToken);
        Assert.Empty(this.Context.ChangeTracker.Entries<Product>());
    }
}