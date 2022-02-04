using System.Net;

using Bogus;

using Microsoft.AspNetCore.Mvc;

using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests.Endpoints.ProductOptions;

public class AddNewOptionForProductTests : IntegrationTestsBase
{
    private readonly Guid productId = Guid.NewGuid();

    public AddNewOptionForProductTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(this.productId)
            .Build();
    }

    [Fact]
    public async Task CanAddNewProductOption()
    {
        var createModel = CreateOptionModel();
        var route = string.Format(Constants.Routes.ProductOptions.Create, this.productId);
        var response = await this.Post<AddNewOptionForProductResponse>(route, createModel);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Headers!.Location);

        await using var context = this.CreateContext();
        var option = context.ProductOptions.Single(x => x.Id == response.Data.OptionId);

        Assert.Equal(createModel.Description, option.Description);
        Assert.Equal(createModel.Name, option.Name);
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Create, this.productId);
        var (result, statusCode) = await this.Post<ProblemDetails>(route, CreateOptionModel(), Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task Returns400IfProductIdNotInvalid()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Create, Guid.Empty);
        var (result, statusCode) = await this.Post<ProblemDetails>(route, CreateOptionModel());

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns400IfOptionPropertiesNotValid()
    {
        var createModel = new OptionModel
        {
            Description = new('x', 24),
            Name = new('x', 10),
        };
        var route = string.Format(Constants.Routes.ProductOptions.Create, this.productId);
        var (result, statusCode) = await this.Post<ProblemDetails>(route, createModel);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductDoesNotExist()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Create, Guid.NewGuid());
        var (result, statusCode) = await this.Post<ProblemDetails>(route, CreateOptionModel());

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    private static OptionModel CreateOptionModel()
    {
        var faker = new Faker<OptionModel>();

        faker
            .RuleFor(x => x.Name, x => x.Random.String2(1, 9))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 23));

        return faker.Generate();
    }

    private class AddNewOptionForProductResponse
    {
        public Guid OptionId { get; init; }
    }

    private class OptionModel
    {
        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }
    }
}