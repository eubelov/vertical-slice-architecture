namespace ProductsApi.DataAccess.Entities;

public sealed class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal DeliveryPrice { get; set; }

    public List<ProductOption> ProductOptions { get; set; } = new();
}