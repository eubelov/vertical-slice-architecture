using Ardalis.Specification;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.Features.Validators;

public sealed class FindProductOptionByIdSpec : Specification<ProductOption>
{
    public FindProductOptionByIdSpec(Guid productOptionId)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Id == productOptionId);
    }
}