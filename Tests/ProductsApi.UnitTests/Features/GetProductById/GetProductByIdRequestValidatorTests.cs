using FluentValidation.TestHelper;

using RefactorThis.Features.GetProductById;
using RefactorThis.Features.Validators;
using RefactorThis.Models;

using Xunit;

namespace RefactorThis.UnitTests.Features.GetProductById;

public class GetProductByIdRequestValidatorTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly GetProductByIdRequestValidator validator;

    public GetProductByIdRequestValidatorTests()
    {
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.Mocker.Use(typeof(ProductExistsValidator), new ProductExistsValidator(this.ReadOnlyEntityService));
        this.validator = this.Mocker.CreateInstance<GetProductByIdRequestValidator>();
    }

    [Fact]
    public async Task AcceptsValidRequest()
    {
        var result = await this.validator.TestValidateAsync(new() { ProductId = this.existingProductId });
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ReturnsErrorIfProductDoesNotExistInDB()
    {
        var result = await this.validator.TestValidateAsync(new() { ProductId = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor("Product");
        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public async Task ReturnsErrorIfProductIdEmpty()
    {
        var result = await this.validator.TestValidateAsync(new() { ProductId = Guid.Empty });
        result.ShouldHaveValidationErrorFor("ProductId");
    }
}