using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.UpdateProduct;

public sealed class UpdateProductRequest : IRequest<MediatorResponse<Unit>>
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public decimal Price { get; init; }

    public decimal DeliveryPrice { get; init; }
}