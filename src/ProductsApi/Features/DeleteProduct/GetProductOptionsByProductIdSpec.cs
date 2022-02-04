using Ardalis.Specification;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.Features.DeleteProduct;

public sealed class GetProductOptionsByProductIdSpec : Specification<ProductOption>
{
    public GetProductOptionsByProductIdSpec(Guid productId)
    {
        this.Query
            .Where(x => x.ProductId == productId);
    }
}