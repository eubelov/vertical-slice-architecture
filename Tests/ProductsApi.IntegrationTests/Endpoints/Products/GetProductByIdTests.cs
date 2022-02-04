using System.Net;

using Bogus;

using Microsoft.AspNetCore.Mvc;

using ProductsApi.DataAccess.Entities;
using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests.Endpoints.Products;

public class GetProductByIdTests : IntegrationTestsBase
{
    private readonly Product product;

    public GetProductByIdTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        var faker = new Faker<Product>();
        faker
            .RuleFor(x => x.Id, x => x.Random.Guid())
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Random.String2(1, 10))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 35));

        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(this.product = faker.Generate())
            .Build();
    }

    [Fact]
    public async Task CanGetProductById()
    {
        var (result, statusCode) = await this.Get<GetProductByIdResponse>(string.Format(Constants.Routes.Products.GetById, this.product.Id));
        Assert.Equal(HttpStatusCode.OK, statusCode);

        Assert.NotNull(result);
        Assert.Equal(this.product.Id, result.Id);
        Assert.Equal(this.product.Description, result.Description);
        Assert.Equal(this.product.Name, result.Name);
        Assert.Equal(this.product.Price, result.Price);
        Assert.Equal(this.product.DeliveryPrice, result.DeliveryPrice);
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.Products.GetById, this.product.Id);
        var (result, statusCode) = await this.Get<ProblemDetails>(route, Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task Returns400IfProductIdNotInvalid()
    {
        var route = string.Format(Constants.Routes.Products.GetById, Guid.Empty);
        var (result, statusCode) = await this.Get<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductDoesNotExist()
    {
        var route = string.Format(Constants.Routes.Products.GetById, Guid.NewGuid());
        var (result, statusCode) = await this.Get<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    private class GetProductByIdResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }

        public decimal Price { get; init; }

        public decimal DeliveryPrice { get; init; }
    }
}