using Mapster;

using MediatR;

using ProductsApi.DataAccess.Entities;
using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.FindProducts;

internal sealed class FindProductsRequestHandler : IRequestHandler<FindProductsRequest, MediatorResponse<FindProductsResponse>>
{
    private readonly IReadOnlyEntityService entityService;

    public FindProductsRequestHandler(IReadOnlyEntityService entityService)
    {
        this.entityService = entityService;
    }

    public async Task<MediatorResponse<FindProductsResponse>> Handle(FindProductsRequest request, CancellationToken cancellationToken)
    {
        var products = Array.Empty<Product>();
        if (request.Name != null)
        {
            products = await this.entityService.LoadAll(new FindProductsByNameSpec(request.Name), cancellationToken);
        }

        return new()
        {
            Result = new()
            {
                Items = products.Adapt<List<FindProductsResponse.Product>>(),
            },
        };
    }
}