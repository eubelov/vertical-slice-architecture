using ProductsApi.DataAccess.Entities;
using ProductsApi.Features.Validators;

using Xunit;

namespace ProductsApi.UnitTests.Features.Validators;

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