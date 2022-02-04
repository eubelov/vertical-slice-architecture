using FluentValidation;

namespace ProductsApi.Features.CreateProduct;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
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