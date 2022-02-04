using Bogus;

using RefactorThis.Features.AddNewOptionForProduct;

using Xunit;

namespace RefactorThis.UnitTests.Features.AddNewOptionForProduct;

public class AddNewOptionForProductRequestHandlerTests : UnitTestsBaseWithInMemoryContext
{
    private readonly AddNewOptionForProductRequestHandler handler;

    public AddNewOptionForProductRequestHandlerTests()
    {
        this.UseNullLoggerFor<AddNewOptionForProductRequestHandler>();
        this.handler = this.Mocker.CreateInstance<AddNewOptionForProductRequestHandler>();
    }

    [Fact]
    public async Task CanAddNewOptionForProduct()
    {
        var request = CreateRequest();

        var result = await this.handler.Handle(request, AnyToken);

        Assert.NotNull(result.Result);
        Assert.NotEqual(Guid.Empty, result.Result.OptionId);
    }

    [Fact]
    public async Task CreatedProductOptionHasCorrectPropertyValues()
    {
        var request = CreateRequest();

        await this.handler.Handle(request, AnyToken);

        var option = this.Context.ProductOptions.Single();
        Assert.Equal(request.ProductId, option.ProductId);
        Assert.Equal(request.Option.Description, option.Description);
        Assert.Equal(request.Option.Name, option.Name);
    }

    private static AddNewOptionForProductRequest CreateRequest()
    {
        var productId = Guid.NewGuid();
        var optionFaker = new Faker<AddNewOptionForProductRequest.OptionModel>();
        optionFaker
            .RuleFor(x => x.Name, x => x.Commerce.Product())
            .RuleFor(x => x.Description, x => x.Commerce.ProductDescription());

        return new()
        {
            Option = optionFaker.Generate(),
            ProductId = productId,
        };
    }
}