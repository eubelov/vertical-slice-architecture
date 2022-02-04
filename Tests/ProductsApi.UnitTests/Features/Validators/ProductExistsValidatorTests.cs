using FluentValidation.TestHelper;

using Moq;

using RefactorThis.DataAccess.EntityService;
using RefactorThis.Features.Validators;
using RefactorThis.Models;

using Xunit;

namespace RefactorThis.UnitTests.Features.Validators;

public class ProductExistsValidatorTests : UnitTestBase
{
    private readonly ProductExistsValidator validator;

    private readonly Mock<IReadOnlyEntityService> entityService;

    public ProductExistsValidatorTests()
    {
        this.entityService = this.Mocker.GetMock<IReadOnlyEntityService>();

        this.entityService
            .Setup(x => x.Exists(It.IsAny<GetProductByIdSpec>(), AnyToken))
            .ReturnsAsync(true);

        this.validator = this.Mocker.CreateInstance<ProductExistsValidator>();
    }

    [Fact]
    public void ReturnsFalseIfProductDoesNotExist()
    {
        this.entityService
            .Setup(x => x.Exists(It.IsAny<GetProductByIdSpec>(), AnyToken))
            .ReturnsAsync(false);

        var result = this.validator.TestValidate(Guid.NewGuid());
        result.ShouldHaveValidationErrorFor("Product");
        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public void ReturnsTrueIfProductExists()
    {
        var result = this.validator.TestValidate(Guid.NewGuid());
        result.ShouldNotHaveAnyValidationErrors();

        this.entityService.Verify(x => x.Exists(It.IsAny<GetProductByIdSpec>(), AnyToken), Times.Once);
        this.entityService.VerifyNoOtherCalls();
    }
}