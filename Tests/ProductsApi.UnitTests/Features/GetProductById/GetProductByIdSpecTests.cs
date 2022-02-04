using RefactorThis.DataAccess.Entities;
using RefactorThis.Features.GetProductById;

using Xunit;

namespace RefactorThis.UnitTests.Features.GetProductById;

public class GetProductByIdSpecTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid productId = Guid.NewGuid();

    public GetProductByIdSpecTests()
    {
        this.Context.Add(new Product { Id = this.productId, Name = "Name" });
        this.Context.Add(new Product { Id = Guid.NewGuid(), Name = "Name2" });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task CanGetProductById()
    {
        await this.ReadOnlyEntityService.Single<Product, GetProductByIdSpec>(new(this.productId), AnyToken);
    }

    [Fact]
    public async Task DoesNotTrackProduct()
    {
        await this.ReadOnlyEntityService.Single<Product, GetProductByIdSpec>(new(this.productId), AnyToken);
        Assert.Empty(this.Context.ChangeTracker.Entries<Product>());
    }
}