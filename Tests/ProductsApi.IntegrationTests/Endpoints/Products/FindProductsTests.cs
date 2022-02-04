using System.Net;

using Bogus;

using Microsoft.AspNetCore.Mvc;

using ProductsApi.DataAccess.Entities;
using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests.Endpoints.Products;

public class FindProductsTests : IntegrationTestsBase
{
    private const string SearchString = "x42";

    private readonly Product product1;

    private readonly Product product2;

    public FindProductsTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        var faker = new Faker<Product>();
        faker
            .RuleFor(x => x.Id, x => x.Random.Guid())
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Random.String2(1, 10))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 35));

        this.product1 = faker.Generate();
        this.product1.Name = $"p{SearchString}{this.product1.Name}";

        this.product2 = faker.Generate();
        this.product2.Name = $"{SearchString}{this.product2.Name}";

        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(faker.Generate())
            .WithProduct(this.product1)
            .WithProduct(this.product2)
            .Build();
    }

    [Fact]
    public async Task CanFindProducts()
    {
        var (result, statusCode) = await this.Get<FindProductsResponse>(string.Format(Constants.Routes.Products.Find, SearchString));
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal(2, result!.Items.Count);

        Assert.True(Equal(this.product1, result.Items.Single(x => x.Id == this.product1.Id)));
        Assert.True(Equal(this.product2, result.Items.Single(x => x.Id == this.product2.Id)));
    }

    [Fact]
    public async Task ReturnsEmptyListIfNameNotSpecified()
    {
        var (result, statusCode) = await this.Get<FindProductsResponse>(string.Format(Constants.Routes.Products.Find, string.Empty));
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Empty(result!.Items);
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.Products.Find, SearchString);
        var (result, statusCode) = await this.Get<ProblemDetails>(route, Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task Returns400IfNameNotInvalid()
    {
        var route = string.Format(Constants.Routes.Products.Find, new string('x', 18));
        var (result, statusCode) = await this.Get<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    private static bool Equal(Product product, FindProductsResponse.Product responseProduct)
    {
        return product.Id == responseProduct.Id
               && product.Description == responseProduct.Description
               && product.Name == responseProduct.Name
               && product.Price == responseProduct.Price
               && product.DeliveryPrice == responseProduct.DeliveryPrice;
    }

    private class FindProductsResponse
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        public List<Product> Items { get; } = new(0);

        public sealed class Product
        {
            public Guid Id { get; init; }

            public string Name { get; init; } = string.Empty;

            public string? Description { get; init; }

            public decimal Price { get; init; }

            public decimal DeliveryPrice { get; init; }
        }
    }
}