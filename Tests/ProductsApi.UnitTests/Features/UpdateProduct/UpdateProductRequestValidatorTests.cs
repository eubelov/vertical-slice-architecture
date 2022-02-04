using Bogus;

using FluentValidation.TestHelper;

using ProductsApi.Features.UpdateProduct;
using ProductsApi.Features.Validators;
using ProductsApi.Models;

using Xunit;

namespace ProductsApi.UnitTests.Features.UpdateProduct;

public class UpdateProductRequestValidatorTests : UnitTestsBaseWithInMemoryContext
{
    private readonly UpdateProductRequestValidator validator;

    private readonly Guid existingProductId = Guid.NewGuid();

    public UpdateProductRequestValidatorTests()
    {
        this.Context.Products.Add(new() { Id = this.existingProductId });
        this.Context.SaveChanges();

        this.Mocker.Use(typeof(ProductExistsValidator), new ProductExistsValidator(this.ReadOnlyEntityService));

        this.validator = this.Mocker.CreateInstance<UpdateProductRequestValidator>();
    }

    [Fact]
    public void ReturnsErrorIfProductIdIsEmpty()
    {
        var result = this.validator.TestValidate(new() { Id = Guid.Empty });
        result.ShouldHaveValidationErrorFor("ProductId");
    }

    [Fact]
    public void ReturnsErrorIfProductDoesNotExistInDb()
    {
        var result = this.validator.TestValidate(new() { Id = Guid.NewGuid() });
        result.ShouldHaveValidationErrorFor("Product");

        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public void ReturnsErrorIfDescriptionIsTooLong()
    {
        var result = this.validator.TestValidate(new() { Id = this.existingProductId, Name = "name", Description = new('x', 36) });
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ReturnsErrorIfNameIsEmpty()
    {
        var result = this.validator.TestValidate(new() { Id = this.existingProductId, Name = string.Empty });
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ReturnsErrorIfNameIsTooLong()
    {
        var result = this.validator.TestValidate(new() { Id = this.existingProductId, Name = new('x', 18) });
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ReturnsErrorIfPriceLessThanZero()
    {
        var result = this.validator.TestValidate(new() { Id = this.existingProductId, Name = "name", Price = -1 });
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void AcceptsValidRequest()
    {
        var faker = new Faker<UpdateProductRequest>();
        faker
            .RuleFor(x => x.Id, _ => this.existingProductId)
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Random.String2(1, 17))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 35));

        var result = this.validator.TestValidate(faker.Generate());
        result.ShouldNotHaveAnyValidationErrors();
    }
}