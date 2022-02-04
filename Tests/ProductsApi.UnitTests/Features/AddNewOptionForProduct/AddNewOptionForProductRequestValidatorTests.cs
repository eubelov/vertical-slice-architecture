using FluentValidation.TestHelper;

using RefactorThis.Features.AddNewOptionForProduct;
using RefactorThis.Features.Validators;
using RefactorThis.Models;

using Xunit;

namespace RefactorThis.UnitTests.Features.AddNewOptionForProduct;

public class AddNewOptionForProductRequestValidatorTests : UnitTestsBaseWithInMemoryContext
{
    private readonly Guid existingProductId = Guid.NewGuid();

    private readonly AddNewOptionForProductRequestValidator validator;

    public AddNewOptionForProductRequestValidatorTests()
    {
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.Mocker.Use(typeof(ProductExistsValidator), new ProductExistsValidator(this.ReadOnlyEntityService));

        this.validator = this.Mocker.CreateInstance<AddNewOptionForProductRequestValidator>();
    }

    [Fact]
    public async Task AcceptsValidRequest()
    {
        var request = new AddNewOptionForProductRequest
        {
            ProductId = this.existingProductId,
            Option = new()
            {
                Name = "Option",
                Description = "New option",
            },
        };

        var result = await this.validator.TestValidateAsync(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ReturnsErrorIfOptionDescriptionTooLong()
    {
        var request = new AddNewOptionForProductRequest
        {
            ProductId = this.existingProductId,
            Option = new()
            {
                Name = "Option",
                Description = new('x', 24),
            },
        };

        var result = await this.validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Option.Description);
    }

    [Fact]
    public async Task ReturnsErrorIfOptionNameIsEmpty()
    {
        var result = await this.validator.TestValidateAsync(
                         new() { ProductId = this.existingProductId, Option = new() { Name = string.Empty } });
        result.ShouldHaveValidationErrorFor(x => x.Option.Name);
    }

    [Fact]
    public async Task ReturnsErrorIfOptionNameIsTooLong()
    {
        var result = await this.validator.TestValidateAsync(
                         new() { ProductId = this.existingProductId, Option = new() { Name = "1234567891011" } });
        result.ShouldHaveValidationErrorFor(x => x.Option.Name);
    }

    [Fact]
    public async Task ReturnsErrorIfProductDoesNotExistInDB()
    {
        var request = new AddNewOptionForProductRequest
        {
            ProductId = Guid.NewGuid(),
            Option = new()
            {
                Name = "Option",
                Description = "New option",
            },
        };

        var result = await this.validator.TestValidateAsync(request);
        result.ShouldHaveValidationErrorFor("Product");
        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public async Task ReturnsErrorIfProductIdEmpty()
    {
        var result = await this.validator.TestValidateAsync(new() { ProductId = Guid.Empty });
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }
}