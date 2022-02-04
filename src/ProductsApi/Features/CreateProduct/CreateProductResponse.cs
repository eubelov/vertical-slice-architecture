namespace RefactorThis.Features.CreateProduct;

public sealed class CreateProductResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public decimal Price { get; init; }

    public decimal DeliveryPrice { get; init; }
}