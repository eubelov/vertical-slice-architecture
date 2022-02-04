namespace RefactorThis.DataAccess.Entities;

public sealed class ProductOption
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}