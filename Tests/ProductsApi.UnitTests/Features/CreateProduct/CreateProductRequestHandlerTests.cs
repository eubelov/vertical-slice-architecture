using Bogus;

using ProductsApi.Features.CreateProduct;

using Xunit;

namespace ProductsApi.UnitTests.Features.CreateProduct;

public class CreateProductRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly CreateProductRequestHandler handler;

    public CreateProductRequestHandlerTests()
    {
        this.UseNullLoggerFor<CreateProductRequestHandler>();
        this.handler = this.Mocker.CreateInstance<CreateProductRequestHandler>();
    }

    [Fact]
    public async Task CanAddNewProduct()
    {
        var request = CreateRequest();

        await this.handler.Handle(request, AnyToken);
        Assert.Single(this.Context.Products);
    }

    [Fact]
    public async Task ResultHasCorrectPropertiesValues()
    {
        var request = CreateRequest();

        var result = await this.handler.Handle(request, AnyToken);

        Assert.NotNull(result.Result);

        var product = result.Result;
        Assert.Equal(request.Description, product.Description);
        Assert.Equal(request.Name, product.Name);
        Assert.Equal(request.Price, product.Price);
        Assert.Equal(request.DeliveryPrice, product.DeliveryPrice);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public async Task CreatedProductHasCorrectPropertiesValues()
    {
        var request = CreateRequest();

        await this.handler.Handle(request, AnyToken);

        var product = this.Context.Products.Single();
        Assert.Equal(request.Description, product.Description);
        Assert.Equal(request.Name, product.Name);
        Assert.Equal(request.Price, product.Price);
        Assert.Equal(request.DeliveryPrice, product.DeliveryPrice);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    private static CreateProductRequest CreateRequest()
    {
        var faker = new Faker<CreateProductRequest>();
        faker
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Commerce.Product())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription());

        return faker.Generate();
    }
}