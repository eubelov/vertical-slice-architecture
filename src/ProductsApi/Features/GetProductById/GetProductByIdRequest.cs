using MediatR;

using ProductsApi.Models;

namespace ProductsApi.Features.GetProductById;

public sealed class GetProductByIdRequest : IRequest<MediatorResponse<GetProductByIdResponse>>
{
    public Guid ProductId { get; init; }
}