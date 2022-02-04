using MediatR;

using RefactorThis.DataAccess.Entities;
using RefactorThis.DataAccess.EntityService;
using RefactorThis.Models;

namespace RefactorThis.Features.AddNewOptionForProduct;

internal sealed class AddNewOptionForProductRequestHandler
    : IRequestHandler<AddNewOptionForProductRequest, MediatorResponse<AddNewOptionForProductResponse>>
{
    private readonly IEntityService entityService;

    private readonly ILogger<AddNewOptionForProductRequestHandler> logger;

    public AddNewOptionForProductRequestHandler(IEntityService entityService, ILogger<AddNewOptionForProductRequestHandler> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }

    public async Task<MediatorResponse<AddNewOptionForProductResponse>> Handle(AddNewOptionForProductRequest request, CancellationToken token)
    {
        var newOption = new ProductOption
        {
            Description = request.Option.Description,
            Name = request.Option.Name,
            ProductId = request.ProductId,
        };

        await this.entityService.Create(newOption, token);
        this.logger.LogInformation("Added new option ID '{optionId}' for product ID '{productId}'", newOption.Id, request.ProductId);

        return new()
        {
            Result = new()
            {
                OptionId = newOption.Id,
            },
        };
    }
}