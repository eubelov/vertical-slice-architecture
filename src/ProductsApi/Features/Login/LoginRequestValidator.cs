using FluentValidation;

namespace ProductsApi.Features.Login;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        this.RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(8);

        this.RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(100);
    }
}