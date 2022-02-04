using FluentValidation;

using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.Validators;

public sealed class ProductExistsValidator : AbstractValidator<Guid>
{
    public ProductExistsValidator(IReadOnlyEntityService entityService)
    {
        this.RuleFor(x => x)
            .MustAsync(
                async (productId, _, context, token) =>
                    {
                        if (await entityService.Exists(new GetProductByIdSpec(productId), token))
                        {
                            return true;
                        }

                        context.AddFailure(new EntityNotFoundValidationFailure("Product", productId));
                        return false;
                    })
            .OverridePropertyName("Product");
    }
}