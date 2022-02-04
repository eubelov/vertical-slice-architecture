using Bogus;

using ProductsApi.DataAccess.Entities;
using ProductsApi.Features.FindProducts;

using Xunit;

namespace ProductsApi.UnitTests.Features.FindProducts;

public class FindProductsRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly FindProductsRequestHandler handler;

    private readonly Product product;

    public FindProductsRequestHandlerTests()
    {
        var faker = new Faker<Product>();
        faker
            .RuleFor(x => x.Id, x => x.Random.Guid())
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => "test" + x.Commerce.Product())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription());

        this.Context.Products.Add(this.product = faker.Generate());
        this.Context.Products.Add(faker.Generate());

        var p3 = faker.Generate();
        p3.Name = "Name";
        this.Context.Products.Add(p3);
        this.Context.SaveChanges();

        this.handler = this.Mocker.CreateInstance<FindProductsRequestHandler>();
    }

    [Fact]
    public async Task ReturnEmptyListIfSearchStringIsNull()
    {
        var result = await this.handler.Handle(new() { Name = null }, AnyToken);
        Assert.NotNull(result.Result);
        Assert.Empty(result.Result.Items);
    }

    [Fact]
    public async Task ReturnEmptyListIfNoMatchingProducts()
    {
        var result = await this.handler.Handle(new() { Name = "super" }, AnyToken);
        Assert.NotNull(result.Result);
        Assert.Empty(result.Result.Items);
    }

    [Theory]
    [InlineData("est")]
    [InlineData("test")]
    [InlineData("t")]
    public async Task CanFindProducts(string search)
    {
        var result = await this.handler.Handle(new() { Name = search }, AnyToken);
        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Items.Count);
    }

    [Fact]
    public async Task ResultPropertiesHaveCorrectValues()
    {
        var result = await this.handler.Handle(new() { Name = "test" }, AnyToken);
        var mappedProduct = result.Result!.Items.Single(x => x.Id == this.product.Id);

        Assert.Equal(this.product.Id, mappedProduct.Id);
        Assert.Equal(this.product.Description, mappedProduct.Description);
        Assert.Equal(this.product.Name, mappedProduct.Name);
        Assert.Equal(this.product.Price, mappedProduct.Price);
        Assert.Equal(this.product.DeliveryPrice, mappedProduct.DeliveryPrice);
    }
}