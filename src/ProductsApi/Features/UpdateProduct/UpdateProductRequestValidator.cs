using FluentValidation;

using RefactorThis.Features.Validators;

namespace RefactorThis.Features.UpdateProduct;

public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator(ProductExistsValidator productExistsValidator)
    {
        this.CascadeMode = CascadeMode.Stop;

        this.RuleFor(x => x.Id)
            .NotEmpty()
            .SetValidator(productExistsValidator)
            .OverridePropertyName("ProductId");

        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(17);

        this.RuleFor(x => x.Description)
            .MaximumLength(35);

        this.RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);

        this.RuleFor(x => x.DeliveryPrice)
            .GreaterThanOrEqualTo(0);
    }
}