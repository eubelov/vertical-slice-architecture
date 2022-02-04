using Ardalis.Specification;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.Features.FindOptionsForProduct;

public sealed class GetProductOptionsByProductIdSpec : Specification<ProductOption>
{
    public GetProductOptionsByProductIdSpec(Guid productId)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.ProductId == productId);
    }
}