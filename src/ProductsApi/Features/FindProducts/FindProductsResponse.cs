namespace RefactorThis.Features.FindProducts;

public sealed class FindProductsResponse
{
    public List<Product> Items { get; init; } = new(0);

    public sealed class Product
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }

        public decimal Price { get; init; }

        public decimal DeliveryPrice { get; init; }
    }
}