using MediatR;

using RefactorThis.Attributes;
using RefactorThis.Models;

namespace RefactorThis.Features.DeleteProduct;

[TransactionScope]
public sealed class DeleteProductRequest : IRequest<MediatorResponse<Unit>>
{
    public Guid ProductId { get; init; }
}