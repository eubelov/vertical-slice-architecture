using Ardalis.Specification;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.Features.FindOptionsForProduct;

public sealed class GetProductOptionsByProductIdSpec : Specification<ProductOption>
{
    public GetProductOptionsByProductIdSpec(Guid productId)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.ProductId == productId);
    }
}