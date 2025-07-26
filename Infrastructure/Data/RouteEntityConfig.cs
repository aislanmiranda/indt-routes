using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class RouteEntityConfig : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.Property(p => p.Id)
         .ValueGeneratedOnAdd()
         .HasColumnOrder(0)
         .HasColumnName("Id")
         .HasColumnType("integer")
         .HasComment("Id para registro da rota");

        builder
            .Property(p => p.Origin)
            .HasColumnOrder(1)
            .HasColumnName("Origin")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(3)
            .HasComment("Origem da rota");

        builder
            .Property(p => p.Destination)
            .HasColumnOrder(2)
            .HasColumnName("Destino")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(3)
            .HasComment("Destino da rota");

        builder
            .ToTable("Route", "public")
            .HasKey(c => c.Id)
            .HasName("pk_route");
    }
}