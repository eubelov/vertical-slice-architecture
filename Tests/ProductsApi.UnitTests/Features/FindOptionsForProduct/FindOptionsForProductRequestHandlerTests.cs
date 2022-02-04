using ProductsApi.DataAccess.Entities;
using ProductsApi.Features.FindOptionsForProduct;

using Xunit;

namespace ProductsApi.UnitTests.Features.FindOptionsForProduct;

public class FindOptionsForProductRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly FindOptionsForProductRequestHandler handler;

    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly ProductOption option;

    public FindOptionsForProductRequestHandlerTests()
    {
        this.option = new()
        {
            Id = Guid.NewGuid(),
            ProductId = this.existingProductId,
            Description = "Decs 1",
            Name = "Name 1",
        };

        this.Context.ProductOptions.Add(this.option);

        this.Context.ProductOptions.Add(
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = this.existingProductId,
                Description = "Decs 2",
                Name = "Name 2",
            });

        this.Context.ProductOptions.Add(
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Description = "Decs 3",
                Name = "Name 3",
            });
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.UseNullLoggerFor<FindOptionsForProductRequestHandler>();
        this.handler = this.Mocker.CreateInstance<FindOptionsForProductRequestHandler>();
    }

    [Fact]
    public async Task CanFindAllOptionsForProduct()
    {
        var result = await this.handler.Handle(new() { ProductId = this.existingProductId }, AnyToken);

        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Items.Count);
        Assert.True(result.Result.Items.All(x => x.ProductId == this.existingProductId));
    }

    [Fact]
    public async Task ResultPropertiesHaveCorrectValues()
    {
        var result = await this.handler.Handle(new() { ProductId = this.existingProductId }, AnyToken);

        var mappedOption = result.Result!.Items.Single(x => x.Id == this.option.Id);
        Assert.Equal(this.option.Description, mappedOption.Description);
        Assert.Equal(this.option.Name, mappedOption.Name);
        Assert.Equal(this.option.ProductId, mappedOption.ProductId);
    }

    [Fact]
    public async Task ReturnsEmptyListForProductWithoutOptions()
    {
        var result = await this.handler.Handle(new() { ProductId = Guid.NewGuid() }, AnyToken);

        Assert.Empty(result.Result!.Items);
    }
}