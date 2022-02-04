using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.FindProducts;

public sealed class FindProductsRequest : IRequest<MediatorResponse<FindProductsResponse>>
{
    public string? Name { get; init; }
}