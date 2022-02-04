using MediatR;

using RefactorThis.Models;

namespace RefactorThis.Features.AddNewOptionForProduct;

public sealed class AddNewOptionForProductRequest : IRequest<MediatorResponse<AddNewOptionForProductResponse>>
{
    public Guid ProductId { get; init; }

    public OptionModel Option { get; init; } = new();

    public sealed class OptionModel
    {
        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }
    }
}