using System.Net;

using Bogus;

using Microsoft.AspNetCore.Mvc;

using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests.Endpoints.Products;

public class CreateProductTests : IntegrationTestsBase
{
    public CreateProductTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .Build();
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var product = GetRequest();
        var (result, statusCode) = await this.Post<ProblemDetails>(Constants.Routes.Products.Create, product, Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task Returns400IfCreateModelHasInvalidValues()
    {
        var product = GetRequest();
        product.Name = new('x', 32);
        var (result, statusCode) = await this.Post<ProblemDetails>(Constants.Routes.Products.Create, product);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task CanCreateProduct()
    {
        var product = GetRequest();
        var response = await this.Post<CreateProductResponse>(Constants.Routes.Products.Create, product);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers!.Location);

        var createdProduct = response.Data;
        Assert.NotNull(createdProduct);
        Assert.Equal(product.Description, createdProduct.Description);
        Assert.Equal(product.Name, createdProduct.Name);
        Assert.Equal(product.Price, createdProduct.Price);
        Assert.Equal(product.DeliveryPrice, createdProduct.DeliveryPrice);

        await using var context = this.CreateContext();
        var productInDb = context.Products.Single(x => x.Id == createdProduct.Id);

        Assert.Equal(product.Description, productInDb.Description);
        Assert.Equal(product.Name, productInDb.Name);
        Assert.Equal(product.Price, productInDb.Price);
        Assert.Equal(product.DeliveryPrice, productInDb.DeliveryPrice);
    }

    private static CreateProductModel GetRequest()
    {
        var faker = new Faker<CreateProductModel>();
        faker
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Random.String2(1, 17))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 35));

        return faker.Generate();
    }

    private class CreateProductResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }

        public decimal Price { get; init; }

        public decimal DeliveryPrice { get; init; }
    }

    private class CreateProductModel
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}