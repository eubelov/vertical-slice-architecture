using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.FindOptionsForProduct;

public sealed class FindOptionsForProductRequest : IRequest<MediatorResponse<FindOptionsForProductResponse>>
{
    public Guid ProductId { get; init; }
}