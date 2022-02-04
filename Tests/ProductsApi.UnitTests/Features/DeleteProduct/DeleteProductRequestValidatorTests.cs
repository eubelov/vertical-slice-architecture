using FluentValidation.TestHelper;

using ProductsApi.Features.DeleteProduct;
using ProductsApi.Features.Validators;
using ProductsApi.Models;

using Xunit;

namespace ProductsApi.UnitTests.Features.DeleteProduct;

public class DeleteProductRequestValidatorTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly DeleteProductRequestValidator validator;

    public DeleteProductRequestValidatorTests()
    {
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.Mocker.Use(typeof(ProductExistsValidator), new ProductExistsValidator(this.ReadOnlyEntityService));
        this.validator = this.Mocker.CreateInstance<DeleteProductRequestValidator>();
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