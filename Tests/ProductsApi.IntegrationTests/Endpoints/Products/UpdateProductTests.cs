using System.Net;

using Bogus;

using Microsoft.AspNetCore.Mvc;

using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests.Endpoints.Products;

public class UpdateProductTests : IntegrationTestsBase
{
    private readonly Guid productId = Guid.NewGuid();

    public UpdateProductTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(this.productId)
            .Build();
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.Products.Update, this.productId);
        var (result, statusCode) = await this.Put<ProblemDetails>(route, GetUpdateModel(), Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task Returns400IfUpdateModelHasInvalidProperties()
    {
        var updateModel = GetUpdateModel();
        updateModel.Name = new('x', 18);
        updateModel.Description = new('x', 36);
        var route = string.Format(Constants.Routes.Products.Delete, Guid.Empty);
        var (result, statusCode) = await this.Put<ProblemDetails>(route, updateModel);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductDoesNotExist()
    {
        var route = string.Format(Constants.Routes.Products.Delete, Guid.NewGuid());
        var (result, statusCode) = await this.Put<ProblemDetails>(route, GetUpdateModel());

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    [Fact]
    public async Task CanUpdateProduct()
    {
        var updateModel = GetUpdateModel();
        var (_, statusCode) = await this.Put<object>(string.Format(Constants.Routes.Products.Update, this.productId), updateModel);

        Assert.Equal(HttpStatusCode.NoContent, statusCode);

        await using var context = this.CreateContext();
        var productInDb = context.Products.Single();

        Assert.Equal(updateModel.Description, productInDb.Description);
        Assert.Equal(updateModel.Name, productInDb.Name);
        Assert.Equal(updateModel.Price, productInDb.Price);
        Assert.Equal(updateModel.DeliveryPrice, productInDb.DeliveryPrice);
    }

    private static ProductUpdateModel GetUpdateModel()
    {
        var faker = new Faker<ProductUpdateModel>();
        faker
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Random.String2(1, 17))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 35));

        return faker.Generate();
    }

    private class ProductUpdateModel
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}