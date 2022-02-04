using FluentValidation;

using ProductsApi.Features.Validators;

namespace ProductsApi.Features.GetProductById;

public sealed class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
{
    public GetProductByIdRequestValidator(ProductExistsValidator productExistsValidator)
    {
        this.CascadeMode = CascadeMode.Stop;

        this.RuleFor(x => x.ProductId)
            .NotEmpty()
            .SetValidator(productExistsValidator)
            .OverridePropertyName("ProductId");
    }
}