using Ardalis.Specification;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.Features.DeleteProduct;

public sealed class GetProductOptionsByProductIdSpec : Specification<ProductOption>
{
    public GetProductOptionsByProductIdSpec(Guid productId)
    {
        this.Query
            .Where(x => x.ProductId == productId);
    }
}