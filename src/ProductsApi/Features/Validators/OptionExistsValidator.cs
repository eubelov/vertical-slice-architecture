using FluentValidation;

using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.Validators;

public sealed class OptionExistsValidator : AbstractValidator<Guid>
{
    public OptionExistsValidator(IReadOnlyEntityService entityService)
    {
        this.RuleFor(x => x)
            .MustAsync(
                async (optionId, _, context, token) =>
                    {
                        if (await entityService.Exists(new FindProductOptionByIdSpec(optionId), token))
                        {
                            return true;
                        }

                        context.AddFailure(new EntityNotFoundValidationFailure("Product Option", optionId));
                        return false;
                    })
            .OverridePropertyName("Product Option");
    }
}