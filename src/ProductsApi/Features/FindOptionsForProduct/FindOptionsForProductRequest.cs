using MediatR;

using ProductsApi.Models;

namespace ProductsApi.Features.FindOptionsForProduct;

public sealed class FindOptionsForProductRequest : IRequest<MediatorResponse<FindOptionsForProductResponse>>
{
    public Guid ProductId { get; init; }
}