using System.Net;

using Microsoft.AspNetCore.Mvc;

using ProductsApi.DataAccess.Entities;
using ProductsApi.IntegrationTests.Utils;

using Xunit;

namespace ProductsApi.IntegrationTests.Endpoints.ProductOptions;

public class FindOptionsForProduct : IntegrationTestsBase
{
    private readonly Guid productId = Guid.NewGuid();

    private readonly Guid product2Id = Guid.NewGuid();

    private readonly ProductOption option1;

    private readonly ProductOption option2;

    public FindOptionsForProduct(CustomWebApplicationFactory<Startup> factory)
        : base(factory)
    {
        var optionFaker = FakersFactory.GetProductOptionFaker(this.productId);

        this.DatabaseBuilder.Initialize()
            .WithUser()
            .WithProduct(this.productId)
            .WithProduct(this.product2Id)
            .WithProductOption(this.option1 = optionFaker.Generate())
            .WithProductOption(this.option2 = optionFaker.Generate())
            .Build();
    }

    [Fact]
    public async Task CanGetProductOptions()
    {
        var route = string.Format(Constants.Routes.ProductOptions.GetForProduct, this.productId);
        var (result, statusCode) = await this.Get<FindOptionsForProductResponse>(route);

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);

        Assert.True(Equal(this.option1, result.Items.Single(x => x.Id == this.option1.Id)));
        Assert.True(Equal(this.option2, result.Items.Single(x => x.Id == this.option2.Id)));
    }

    [Fact]
    public async Task CanGetEmptyListOfProductOptions()
    {
        var route = string.Format(Constants.Routes.ProductOptions.GetForProduct, this.product2Id);
        var (result, statusCode) = await this.Get<FindOptionsForProductResponse>(route);

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Returns400IfProductIdNotValid()
    {
        var route = string.Format(Constants.Routes.ProductOptions.GetForProduct, Guid.Empty);
        var (result, statusCode) = await this.Get<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        Assert.Equal(Constants.ProblemTypes.ModelValidationError, result!.Type);
    }

    [Fact]
    public async Task Returns404IfProductDoesNotExist()
    {
        var route = string.Format(Constants.Routes.ProductOptions.GetForProduct, Guid.NewGuid());
        var (result, statusCode) = await this.Get<ProblemDetails>(route);

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
        Assert.Equal(Constants.ProblemTypes.NotFound, result!.Type);
    }

    [Fact]
    public async Task Returns401IfApiTokenNotValid()
    {
        var route = string.Format(Constants.Routes.ProductOptions.GetForProduct, this.productId);
        var (result, statusCode) = await this.Get<ProblemDetails>(route, Guid.NewGuid());

        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        Assert.Equal(Constants.ProblemTypes.Unauthorized, result!.Type);
    }

    private static bool Equal(ProductOption option, FindOptionsForProductResponse.ProductOption responseOption)
    {
        return option.Id == responseOption.Id
               && option.Description == responseOption.Description
               && option.Name == responseOption.Name
               && option.ProductId == responseOption.ProductId;
    }

    private class FindOptionsForProductResponse
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        public List<ProductOption> Items { get; init; } = new();

        public sealed class ProductOption
        {
            public Guid Id { get; init; }

            public Guid ProductId { get; init; }

            public string Name { get; init; } = string.Empty;

            public string? Description { get; init; }
        }
    }
}