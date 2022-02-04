namespace ProductsApi.Features.FindOptionsForProduct;

public sealed class FindOptionsForProductResponse
{
    public List<ProductOption> Items { get; init; } = new();

    public sealed class ProductOption
    {
        public Guid Id { get; init; }

        public Guid ProductId { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }
    }
}