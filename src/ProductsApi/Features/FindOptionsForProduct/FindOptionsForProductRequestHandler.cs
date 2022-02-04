using Mapster;

using MediatR;

using RefactorThis.DataAccess.EntityService;
using RefactorThis.Models;

namespace RefactorThis.Features.FindOptionsForProduct;

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