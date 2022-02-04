using Ardalis.Specification;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.Features.Validators;

public sealed class GetProductByIdSpec : Specification<Product>, ISingleResultSpecification
{
    public GetProductByIdSpec(Guid productId)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Id == productId);
    }
}