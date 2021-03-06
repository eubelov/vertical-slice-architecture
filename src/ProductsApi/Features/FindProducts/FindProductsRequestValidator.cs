using FluentValidation;

namespace ProductsApi.Features.FindProducts;

public sealed class FindProductsRequestValidator : AbstractValidator<FindProductsRequest>
{
    public FindProductsRequestValidator()
    {
        this.RuleFor(x => x.Name)
            .MaximumLength(17);
    }
}