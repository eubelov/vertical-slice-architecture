using Ardalis.Specification;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.Features.Validators;

public sealed class FindProductOptionByIdSpec : Specification<ProductOption>
{
    public FindProductOptionByIdSpec(Guid productOptionId)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Id == productOptionId);
    }
}