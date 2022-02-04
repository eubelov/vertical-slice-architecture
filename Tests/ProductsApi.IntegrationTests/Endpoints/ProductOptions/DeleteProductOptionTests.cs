using System.Net;

using Microsoft.AspNetCore.Mvc;

using RefactorThis.IntegrationTests.Utils;

using Xunit;

namespace RefactorThis.IntegrationTests.Endpoints.ProductOptions;

public class DeleteProductOptionTests : IntegrationTestsBase
{
    private readonly Guid productId = Guid.NewGuid();

    private readonly Guid productOptionId = Guid.NewGuid();

    public DeleteProductOptionTests(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(this.productId)
            .WithProductOption(this.productId, this.productOptionId)
            .Build();
    }

    [Fact]
    public async Task Returns400IfProductIdNotValid()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Delete, Guid.Empty, this.productOptionId);
        var (result, statusCode) = await this.Delete<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductDoesNotExist()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Delete, Guid.NewGuid(), this.productOptionId);
        var (result, statusCode) = await this.Delete<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductOptionDoesNotExist()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Delete, this.productId, Guid.NewGuid());
        var (result, statusCode) = await this.Delete<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Delete, this.productId, this.productOptionId);
        var (result, statusCode) = await this.Delete<ProblemDetails>(route, Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    [Fact]
    public async Task CanDeleteProductOptions()
    {
        var route = string.Format(Constants.Routes.ProductOptions.Delete, this.productId, this.productOptionId);
        var (_, statusCode) = await this.Delete<object>(route);

        Assert.Equal(HttpStatusCode.NoContent, statusCode);

        await using var context = this.CreateContext();

        Assert.False(context.ProductOptions.Any(x => x.Id == this.productOptionId));
    }
}