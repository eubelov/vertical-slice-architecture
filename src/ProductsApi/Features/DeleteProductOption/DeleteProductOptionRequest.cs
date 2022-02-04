using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.DeleteProductOption;

public sealed class DeleteProductOptionRequest : IRequest<MediatorResponse<Unit>>
{
    public Guid ProductId { get; init; }

    public Guid OptionId { get; init; }
}