using Mapster;

using MediatR;

using RefactorThis.DataAccess.Entities;
using RefactorThis.DataAccess.EntityService;
using RefactorThis.Models;

namespace RefactorThis.Features.GetProductById;

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