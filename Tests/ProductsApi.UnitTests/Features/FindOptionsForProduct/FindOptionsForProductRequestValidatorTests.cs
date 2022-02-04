using FluentValidation.TestHelper;

using ProductsApi.Features.FindOptionsForProduct;
using ProductsApi.Features.Validators;
using ProductsApi.Models;

using Xunit;

namespace ProductsApi.UnitTests.Features.FindOptionsForProduct;

public class FindOptionsForProductRequestValidatorTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly FindOptionsForProductRequestValidator validator;

    public FindOptionsForProductRequestValidatorTests()
    {
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.Mocker.Use(typeof(ProductExistsValidator), new ProductExistsValidator(this.ReadOnlyEntityService));

        this.validator = this.Mocker.CreateInstance<FindOptionsForProductRequestValidator>();
    }

    [Fact]
    public void ReturnsErrorIfProductIdIsEmpty()
    {
        var result = this.validator.TestValidate(new() { ProductId = Guid.Empty });
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public void ReturnsErrorIfProductDoesNotExistInDb()
    {
        var result = this.validator.TestValidate(new() { ProductId = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor("Product");

        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public void AcceptsValidRequest()
    {
        var result = this.validator.TestValidate(new() { ProductId = this.existingProductId });
        result.ShouldNotHaveAnyValidationErrors();
    }
}