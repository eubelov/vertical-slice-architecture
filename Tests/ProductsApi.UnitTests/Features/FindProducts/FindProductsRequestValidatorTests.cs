using FluentValidation.TestHelper;

using RefactorThis.Features.FindProducts;

using Xunit;

namespace RefactorThis.UnitTests.Features.FindProducts;

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