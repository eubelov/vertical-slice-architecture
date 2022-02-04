using MediatR;

using ProductsApi.DataAccess.Entities;
using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.DeleteProductOption;

internal sealed class DeleteProductOptionRequestHandler : IRequestHandler<DeleteProductOptionRequest, MediatorResponse<Unit>>
{
    private readonly IEntityService entityService;

    private readonly ILogger<DeleteProductOptionRequestHandler> logger;

    public DeleteProductOptionRequestHandler(IEntityService entityService, ILogger<DeleteProductOptionRequestHandler> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }

    public async Task<MediatorResponse<Unit>> Handle(DeleteProductOptionRequest request, CancellationToken cancellationToken)
    {
        await this.entityService.Remove(new ProductOption { Id = request.OptionId }, cancellationToken);
        this.logger.LogInformation("Deleted option ID '{optionId}' for product ID '{productId}'", request.OptionId, request.ProductId);

        return new();
    }
}