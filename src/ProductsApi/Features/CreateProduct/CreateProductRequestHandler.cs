using Mapster;

using MediatR;

using ProductsApi.DataAccess.Entities;
using ProductsApi.DataAccess.EntityService;
using ProductsApi.Models;

namespace ProductsApi.Features.CreateProduct;

internal sealed class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, MediatorResponse<CreateProductResponse>>
{
    private readonly IEntityService entityService;

    private readonly ILogger<CreateProductRequestHandler> logger;

    public CreateProductRequestHandler(IEntityService entityService, ILogger<CreateProductRequestHandler> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }

    public async Task<MediatorResponse<CreateProductResponse>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = request.Adapt<Product>();
        await this.entityService.Create(product, cancellationToken);
        this.logger.LogInformation("Product with ID '{productId}' created", product.Id);

        return new()
        {
            Result = product.Adapt<CreateProductResponse>(),
        };
    }
}