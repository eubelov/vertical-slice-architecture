using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RefactorThis.DataAccess.Entities;

namespace RefactorThis.DataAccess.EntityConfigurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(17);

        builder.Property(x => x.Description)
            .HasMaxLength(35);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(6,2)");

        builder.Property(x => x.DeliveryPrice)
            .HasColumnType("decimal(4,2)");

        builder.HasMany(x => x.ProductOptions)
            .WithOne()
            .HasForeignKey(x => x.ProductId)
            .IsRequired();
    }
}