using FluentValidation;

using ProductsApi.Features.Validators;

namespace ProductsApi.Features.DeleteProductOption;

public sealed class DeleteProductOptionRequestValidator : AbstractValidator<DeleteProductOptionRequest>
{
    public DeleteProductOptionRequestValidator(ProductExistsValidator productExistsValidator, OptionExistsValidator optionExistsValidator)
    {
        this.CascadeMode = CascadeMode.Stop;

        this.RuleFor(x => x.OptionId)
            .NotEmpty()
            .SetValidator(optionExistsValidator)
            .OverridePropertyName("OptionId");

        this.RuleFor(x => x.ProductId)
            .NotEmpty()
            .SetValidator(productExistsValidator)
            .OverridePropertyName("ProductId");
    }
}