using ProductsApi.DataAccess.Entities;
using ProductsApi.Features.FindOptionsForProduct;

using Xunit;

namespace ProductsApi.UnitTests.Features.FindOptionsForProduct;

public class GetProductOptionsByProductIdSpecTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid productId = Guid.NewGuid();

    public GetProductOptionsByProductIdSpecTests()
    {
        this.Context.Add(new ProductOption { Id = Guid.NewGuid(), Name = "Name", ProductId = this.productId });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task CanGetProductOptions()
    {
        var products = await this.ReadOnlyEntityService.LoadAll(new GetProductOptionsByProductIdSpec(this.productId), AnyToken);
        Assert.Single(products);
    }

    [Fact]
    public async Task DoesNotTrackProductOption()
    {
        await this.ReadOnlyEntityService.LoadAll(new GetProductOptionsByProductIdSpec(this.productId), AnyToken);
        Assert.Empty(this.Context.ChangeTracker.Entries<ProductOption>());
    }
}