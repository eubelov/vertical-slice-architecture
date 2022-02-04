using FluentValidation;

using ProductsApi.Features.Validators;

namespace ProductsApi.Features.DeleteProduct;

public sealed class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator(ProductExistsValidator productExistsValidator)
    {
        this.CascadeMode = CascadeMode.Stop;

        this.RuleFor(x => x.ProductId)
            .NotEmpty()
            .SetValidator(productExistsValidator)
            .OverridePropertyName("ProductId");
    }
}