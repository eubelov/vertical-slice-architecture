using Bogus;

using RefactorThis.DataAccess.Entities;
using RefactorThis.Features.GetProductById;

using Xunit;

namespace RefactorThis.UnitTests.Features.GetProductById;

public class GetProductByIdRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Product product;

    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly GetProductByIdRequestHandler handler;

    public GetProductByIdRequestHandlerTests()
    {
        var faker = new Faker<Product>();
        faker
            .RuleFor(x => x.Id, _ => this.existingProductId)
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Commerce.Product())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription());

        this.Context.Products.Add(this.product = faker.Generate());
        this.Context.SaveChanges();

        this.handler = this.Mocker.CreateInstance<GetProductByIdRequestHandler>();
    }

    [Fact]
    public async Task CanGetProductById()
    {
        var result = await this.handler.Handle(new() { ProductId = this.existingProductId }, AnyToken);
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task ProductPropertiesHaveCorrectValues()
    {
        var result = await this.handler.Handle(new() { ProductId = this.existingProductId }, AnyToken);
        var mappedProduct = result.Result!;

        Assert.Equal(this.product.Id, mappedProduct.Id);
        Assert.Equal(this.product.Description, mappedProduct.Description);
        Assert.Equal(this.product.Name, mappedProduct.Name);
        Assert.Equal(this.product.Price, mappedProduct.Price);
        Assert.Equal(this.product.DeliveryPrice, mappedProduct.DeliveryPrice);
    }
}