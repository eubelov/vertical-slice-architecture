using MediatR;

using ProductsApi.Attributes;
using ProductsApi.Models;

namespace ProductsApi.Features.DeleteProduct;

[TransactionScope]
public sealed class DeleteProductRequest : IRequest<MediatorResponse<Unit>>
{
    public Guid ProductId { get; init; }
}