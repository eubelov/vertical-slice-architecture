using FluentValidation.TestHelper;

using Moq;

using RefactorThis.DataAccess.EntityService;
using RefactorThis.Features.Validators;
using RefactorThis.Models;

using Xunit;

namespace RefactorThis.UnitTests.Features.Validators;

public class OptionExistsValidatorTests : UnitTestBase
{
    private readonly OptionExistsValidator validator;

    private readonly Mock<IReadOnlyEntityService> entityService;

    public OptionExistsValidatorTests()
    {
        this.entityService = this.Mocker.GetMock<IReadOnlyEntityService>();

        this.entityService
            .Setup(x => x.Exists(It.IsAny<FindProductOptionByIdSpec>(), AnyToken))
            .ReturnsAsync(true);

        this.validator = this.Mocker.CreateInstance<OptionExistsValidator>();
    }

    [Fact]
    public void ReturnsFalseIfProductOptionDoesNotExist()
    {
        this.entityService
            .Setup(x => x.Exists(It.IsAny<FindProductOptionByIdSpec>(), AnyToken))
            .ReturnsAsync(false);

        var result = this.validator.TestValidate(Guid.NewGuid());
        result.ShouldHaveValidationErrorFor("Product Option");
        Assert.Single(result.Errors.OfType<EntityNotFoundValidationFailure>());
    }

    [Fact]
    public void ReturnsTrueIfProductOptionExists()
    {
        var result = this.validator.TestValidate(Guid.NewGuid());
        result.ShouldNotHaveAnyValidationErrors();

        this.entityService.Verify(x => x.Exists(It.IsAny<FindProductOptionByIdSpec>(), AnyToken), Times.Once);
        this.entityService.VerifyNoOtherCalls();
    }
}