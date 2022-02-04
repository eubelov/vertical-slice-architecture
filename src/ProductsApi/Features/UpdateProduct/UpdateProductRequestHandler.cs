using Mapster;

using MediatR;

using RefactorThis.DataAccess.Entities;
using RefactorThis.DataAccess.EntityService;
using RefactorThis.Models;

namespace RefactorThis.Features.UpdateProduct;

internal sealed class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, MediatorResponse<Unit>>
{
    private readonly IEntityService entityService;

    private readonly ILogger<UpdateProductRequestHandler> logger;

    public UpdateProductRequestHandler(IEntityService entityService, ILogger<UpdateProductRequestHandler> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }

    public async Task<MediatorResponse<Unit>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        await this.entityService.Update(request.Adapt<Product>(), cancellationToken);
        this.logger.LogInformation("Product with ID '{productId}' updated", request.Id);

        return new();
    }
}