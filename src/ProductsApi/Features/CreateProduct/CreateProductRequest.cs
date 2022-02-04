using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.CreateProduct;

public sealed class CreateProductRequest : IRequest<MediatorResponse<CreateProductResponse>>
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public decimal Price { get; init; }

    public decimal DeliveryPrice { get; init; }
}