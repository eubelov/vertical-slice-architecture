using FluentValidation.TestHelper;

using RefactorThis.Features.DeleteProductOption;
using RefactorThis.Features.Validators;
using RefactorThis.Models;

using Xunit;

namespace RefactorThis.UnitTests.Features.DeleteProductOption;

public class DeleteProductOptionRequestValidatorTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid existingProductOptionId = Guid.NewGuid();

    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly DeleteProductOptionRequestValidator validator;

    public DeleteProductOptionRequestValidatorTests()
    {
        this.Context.ProductOptions.Add(new() { Id = this.existingProductOptionId });
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.Mocker.Use(typeof(ProductExistsValidator), new ProductExistsValidator(this.ReadOnlyEntityService));
        this.Mocker.Use(typeof(OptionExistsValidator), new OptionExistsValidator(this.ReadOnlyEntityService));

        this.validator = this.Mocker.CreateInstance<DeleteProductOptionRequestValidator>();
    }

    [Fact]
    public void ReturnsErrorIfOptionIdIsEmpty()
    {
        var result = this.validator.TestValidate(new() { OptionId = Guid.Empty });
        result.ShouldHaveValidationErrorFor(x => x.OptionId);
    }

    [Fact]
    public void ReturnsErrorIfProductIdIsEmpty()
    {
        var result = this.validator.TestValidate(new() { OptionId = this.existingProductOptionId, ProductId = Guid.Empty });
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public void ReturnsErrorIfProductOptionDoesNotExistInDb()
    {
        var result = this.validator.TestValidate(new() { ProductId = this.existingProductId, OptionId = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor("Product Option");

        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public void ReturnsErrorIfProductDoesNotExistInDb()
    {
        var result = this.validator.TestValidate(new() { ProductId = Guid.NewGuid(), OptionId = this.existingProductOptionId });
        result.ShouldHaveValidationErrorFor("Product");

        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public void AcceptsValidRequest()
    {
        var result = this.validator.TestValidate(new() { ProductId = this.existingProductId, OptionId = this.existingProductOptionId });
        result.ShouldNotHaveAnyValidationErrors();
    }
}