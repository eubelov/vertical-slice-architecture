using Ardalis.Specification;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.Features.FindProducts;

public sealed class FindProductsByNameSpec : Specification<Product>
{
    public FindProductsByNameSpec(string nameSubstring)
    {
        this.Query
            .AsNoTracking()
            .Where(x => x.Name.Contains(nameSubstring));
    }
}