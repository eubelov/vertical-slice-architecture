using ProductsApi.DataAccess.Entities;
using ProductsApi.Features.Validators;

using Xunit;

namespace ProductsApi.UnitTests.Features.Validators;

public class FindProductOptionByIdSpecTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid productOptionId = Guid.NewGuid();

    public FindProductOptionByIdSpecTests()
    {
        this.Context.Add(new ProductOption { Id = this.productOptionId, Name = "Name", ProductId = Guid.NewGuid() });
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task CanGetProductOption()
    {
        Assert.True(await this.ReadOnlyEntityService.Exists(new FindProductOptionByIdSpec(this.productOptionId), AnyToken));
    }

    [Fact]
    public async Task DoesNotTrackProductOption()
    {
        await this.ReadOnlyEntityService.Exists(new FindProductOptionByIdSpec(this.productOptionId), AnyToken);
        Assert.Empty(this.Context.ChangeTracker.Entries<ProductOption>());
    }
}