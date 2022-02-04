using System.Net;

using Microsoft.AspNetCore.Mvc;

using RefactorThis.IntegrationTests.Utils;

using Xunit;

namespace RefactorThis.IntegrationTests.Endpoints.Products;

public class DeleteProductTests : IntegrationTestsBase
{
    private readonly Guid productId = Guid.NewGuid();

    private readonly Guid productOptionId = Guid.NewGuid();

    public DeleteProductTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(this.productId)
            .WithProductOption(this.productId, this.productOptionId)
            .WithProductOption(this.productId, Guid.NewGuid())
            .Build();
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.Products.Delete, this.productId);
        var (result, statusCode) = await this.Delete<ProblemDetails>(route, Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task Returns400IfProductIdNotInvalid()
    {
        var route = string.Format(Constants.Routes.Products.Delete, Guid.Empty);
        var (result, statusCode) = await this.Delete<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductDoesNotExist()
    {
        var route = string.Format(Constants.Routes.Products.Delete, Guid.NewGuid());
        var (result, statusCode) = await this.Delete<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    [Fact]
    public async Task CanDeleteProductWithOptions()
    {
        var (_, statusCode) = await this.Delete<object>(string.Format(Constants.Routes.Products.Delete, this.productId));
        Assert.Equal(HttpStatusCode.NoContent, statusCode);

        await using var context = this.CreateContext();
        Assert.Empty(context.Products);
        Assert.Empty(context.ProductOptions);
    }
}