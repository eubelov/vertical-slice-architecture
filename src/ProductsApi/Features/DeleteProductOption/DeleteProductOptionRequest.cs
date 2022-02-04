using MediatR;

using ProductsApi.Models;

namespace ProductsApi.Features.DeleteProductOption;

public sealed class DeleteProductOptionRequest : IRequest<MediatorResponse<Unit>>
{
    public Guid ProductId { get; init; }

    public Guid OptionId { get; init; }
}