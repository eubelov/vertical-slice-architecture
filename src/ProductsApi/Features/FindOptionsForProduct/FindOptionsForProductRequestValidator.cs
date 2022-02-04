using FluentValidation;

using RefactorThis.Features.Validators;

namespace RefactorThis.Features.FindOptionsForProduct;

public sealed class FindOptionsForProductRequestValidator : AbstractValidator<FindOptionsForProductRequest>
{
    public FindOptionsForProductRequestValidator(ProductExistsValidator productExistsValidator)
    {
        this.CascadeMode = CascadeMode.Stop;

        this.RuleFor(x => x.ProductId)
            .NotEmpty()
            .SetValidator(productExistsValidator)
            .OverridePropertyName("ProductId");
    }
}