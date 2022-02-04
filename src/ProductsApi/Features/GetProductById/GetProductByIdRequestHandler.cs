using Mapster;

using MediatR;

using ProductsApi.DataAccess.Entities;
using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.GetProductById;

internal sealed class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, MediatorResponse<GetProductByIdResponse>>
{
    private readonly IReadOnlyEntityService entityService;

    public GetProductByIdRequestHandler(IReadOnlyEntityService entityService)
    {
        this.entityService = entityService;
    }

    public async Task<MediatorResponse<GetProductByIdResponse>> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await this.entityService.Single<Product, GetProductByIdSpec>(new(request.ProductId), cancellationToken);

        return new()
        {
            Result = product.Adapt<GetProductByIdResponse>(),
        };
    }
}