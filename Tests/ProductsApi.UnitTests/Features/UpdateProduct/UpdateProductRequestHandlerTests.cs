using Bogus;

using ProductsApi.DataAccess.Entities;
using ProductsApi.Features.UpdateProduct;

using Xunit;

namespace ProductsApi.UnitTests.Features.UpdateProduct;

public class UpdateProductRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly UpdateProductRequestHandler handler;

    private readonly Guid productId = Guid.NewGuid();

    public UpdateProductRequestHandlerTests()
    {
        var faker = new Faker<Product>();
        faker
            .RuleFor(x => x.Id, _ => this.productId)
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Commerce.Product())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription());

        this.Context.Products.Add(faker.Generate());
        this.Context.SaveChanges();
        this.Context.ChangeTracker.Clear();

        this.UseNullLoggerFor<UpdateProductRequestHandler>();
        this.handler = this.Mocker.CreateInstance<UpdateProductRequestHandler>();
    }

    [Fact]
    public async Task CanUpdateProduct()
    {
        var request = this.CreateRequest();

        await this.handler.Handle(request, AnyToken);
    }

    [Fact]
    public async Task UpdatedProductHasCorrectPropertiesValues()
    {
        var request = this.CreateRequest();

        await this.handler.Handle(request, AnyToken);

        var updatedProduct = this.Context.Products.Single();
        Assert.Equal(request.Description, updatedProduct.Description);
        Assert.Equal(request.Name, updatedProduct.Name);
        Assert.Equal(request.Price, updatedProduct.Price);
        Assert.Equal(request.DeliveryPrice, updatedProduct.DeliveryPrice);
    }

    private UpdateProductRequest CreateRequest()
    {
        var faker = new Faker<UpdateProductRequest>();
        faker
            .RuleFor(x => x.Id, _ => this.productId)
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Commerce.Product())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription());

        return faker.Generate();
    }
}