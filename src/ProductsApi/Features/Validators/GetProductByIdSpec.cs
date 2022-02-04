using Ardalis.Specification;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.Features.Validators;

public sealed class GetProductByIdSpec : Specification<Product>, ISingleResultSpecification
{
    public GetProductByIdSpec(Guid productId)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Id == productId);
    }
}