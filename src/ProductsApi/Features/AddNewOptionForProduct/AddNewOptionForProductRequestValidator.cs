using FluentValidation;

using ProductsApi.Features.Validators;

namespace ProductsApi.Features.AddNewOptionForProduct;

public sealed class AddNewOptionForProductRequestValidator : AbstractValidator<AddNewOptionForProductRequest>
{
    public AddNewOptionForProductRequestValidator(ProductExistsValidator productExistsValidator)
    {
        this.CascadeMode = CascadeMode.Stop;

        this.RuleFor(x => x.ProductId)
            .NotEmpty()
            .SetValidator(productExistsValidator)
            .OverridePropertyName("ProductId");

        this.RuleFor(x => x.Option)
            .SetValidator(OptionModelValidator.Instance);
    }

    private class OptionModelValidator : AbstractValidator<AddNewOptionForProductRequest.OptionModel>
    {
        public static readonly OptionModelValidator Instance = new();

        private OptionModelValidator()
        {
            this.RuleFor(x => x.Description)
                .MaximumLength(23);

            this.RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(9);
        }
    }
}