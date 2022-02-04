using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProductsApi.DataAccess.Entities;

namespace ProductsApi.DataAccess.EntityConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Login");

        builder.HasKey(x => x.Name);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(x => x.ApiToken)
            .HasColumnName("APIToken");

        builder.Property(x => x.ApiTokenExpiry)
            .IsRequired()
            .HasColumnName("APITokenExpiry");

        builder.HasIndex(x => x.Name, "Login_Name_uindex")
            .IsUnique();
    }
}