using MediatR;

using ProductsApi.Models;

namespace ProductsApi.Features.FindProducts;

public sealed class FindProductsRequest : IRequest<MediatorResponse<FindProductsResponse>>
{
    public string? Name { get; init; }
}