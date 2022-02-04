using Ardalis.Specification;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.Features.FindProducts;

public sealed class FindProductsByNameSpec : Specification<Product>
{
    public FindProductsByNameSpec(string nameSubstring)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Name.Contains(nameSubstring));
    }
}