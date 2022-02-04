using Bogus;

using FluentValidation.TestHelper;

using RefactorThis.Features.CreateProduct;

using Xunit;

namespace RefactorThis.UnitTests.Features.CreateProduct;

public class CreateProductRequestValidatorTests : UnitTestBase
{
    private readonly CreateProductRequestValidator validator;

    public CreateProductRequestValidatorTests()
    {
        this.validator = this.Mocker.CreateInstance<CreateProductRequestValidator>();
    }

    [Fact]
    public void ReturnsErrorIfDeliveryPriceLessThanZero()
    {
        var result = this.validator.TestValidate(new() { DeliveryPrice = -1 });
        result.ShouldHaveValidationErrorFor(x => x.DeliveryPrice);
    }

    [Fact]
    public void ReturnsErrorIfDescriptionIsTooLong()
    {
        var result = this.validator.TestValidate(new() { Description = new('x', 36) });
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void ReturnsErrorIfNameIsEmpty()
    {
        var result = this.validator.TestValidate(new() { Name = string.Empty });
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ReturnsErrorIfNameIsTooLong()
    {
        var result = this.validator.TestValidate(new() { Name = new('x', 18) });
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ReturnsErrorIfPriceLessThanZero()
    {
        var result = this.validator.TestValidate(new() { Price = -1 });
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void AcceptsValidRequest()
    {
        var faker = new Faker<CreateProductRequest>();
        faker
            .RuleFor(x => x.Price, x => x.Random.Decimal())
            .RuleFor(x => x.DeliveryPrice, x => x.Random.Decimal())
            .RuleFor(x => x.Name, x => x.Random.String2(1, 17))
            .RuleFor(x => x.Description, x => x.Random.String2(1, 35));

        var result = this.validator.TestValidate(faker.Generate());
        result.ShouldNotHaveAnyValidationErrors();
    }
}