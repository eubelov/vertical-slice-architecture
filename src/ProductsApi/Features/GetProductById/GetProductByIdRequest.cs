using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.GetProductById;

public sealed class GetProductByIdRequest : IRequest<MediatorResponse<GetProductByIdResponse>>
{
    public Guid ProductId { get; init; }
}