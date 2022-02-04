using FluentValidation.TestHelper;

using RefactorThis.Features.Login;

using Xunit;

namespace RefactorThis.UnitTests.Features.Login;

public class LoginRequestValidatorTests : UnitTestBase
{
    private readonly LoginRequestValidator validator = new();

    [Fact]
    public void ReturnsErrorIfPasswordIsEmpty()
    {
        var result = this.validator.TestValidate(new() { Password = string.Empty });
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void ReturnsErrorIfPasswordTooLong()
    {
        var result = this.validator.TestValidate(new() { Password = new('x', 9) });
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void ReturnsErrorIfUserNameIsEmpty()
    {
        var result = this.validator.TestValidate(new() { UserName = string.Empty });
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void ReturnsErrorIfUserNameTooLong()
    {
        var result = this.validator.TestValidate(new() { UserName = new('x', 101) });
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }
}