using MediatR;

using RefactorThis.DataAccess.Entities;
using RefactorThis.DataAccess.EntityService;
using RefactorThis.Models;

namespace RefactorThis.Features.DeleteProduct;

internal sealed class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, MediatorResponse<Unit>>
{
    private readonly IEntityService entityService;

    private readonly ILogger<DeleteProductRequestHandler> logger;

    public DeleteProductRequestHandler(IEntityService entityService, ILogger<DeleteProductRequestHandler> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }

    public async Task<MediatorResponse<Unit>> Handle(DeleteProductRequest request, CancellationToken token)
    {
        await this.entityService.Remove(new Product { Id = request.ProductId }, token);
        await this.entityService.RemoveSpec(new GetProductOptionsByProductIdSpec(request.ProductId), token);
        this.logger.LogInformation("Product with ID '{productId}' and its options deleted", request.ProductId);

        return new();
    }
}