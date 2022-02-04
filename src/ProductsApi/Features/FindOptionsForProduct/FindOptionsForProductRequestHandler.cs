using Mapster;

using MediatR;

using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.FindOptionsForProduct;

internal sealed class FindOptionsForProductRequestHandler
    : IRequestHandler<FindOptionsForProductRequest, MediatorResponse<FindOptionsForProductResponse>>
{
    private readonly IReadOnlyEntityService entityService;

    public FindOptionsForProductRequestHandler(IReadOnlyEntityService entityService)
    {
        this.entityService = entityService;
    }

    public async Task<MediatorResponse<FindOptionsForProductResponse>> Handle(FindOptionsForProductRequest request, CancellationToken token)
    {
        var options = await this.entityService.LoadAll(new GetProductOptionsByProductIdSpec(request.ProductId), token);

        return new()
        {
            Result = new()
            {
                Items = options.Adapt<List<FindOptionsForProductResponse.ProductOption>>(),
            },
        };
    }
}