using FluentValidation.TestHelper;

using ProductsApi.Features.FindProducts;

using Xunit;

namespace ProductsApi.UnitTests.Features.FindProducts;

public class FindProductsRequestValidatorTests : UnitTestBase
{
    private readonly FindProductsRequestValidator validator = new();

    [Fact]
    public void RejectsTooLongString()
    {
        var result = this.validator.TestValidate(new() { Name = new('x', 18) });
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}