using RefactorThis.DataAccess.Entities;
using RefactorThis.Features.DeleteProduct;

using Xunit;

namespace RefactorThis.UnitTests.Features.DeleteProduct;

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
    public async Task TracksProductOption()
    {
        await this.ReadOnlyEntityService.LoadAll(new GetProductOptionsByProductIdSpec(this.productId), AnyToken);
        Assert.Single(this.Context.ChangeTracker.Entries<ProductOption>());
    }
}