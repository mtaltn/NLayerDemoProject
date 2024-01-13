using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn();

            builder.Property(c => c.Name).IsRequired().HasMaxLength(128);

            builder.Property(c => c.Stock).IsRequired();

            builder.Property(c => c.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.ToTable("Products");

            builder.HasOne(c => c.Category).WithMany(c => c.Products).HasForeignKey(c => c.CategoryId);
        }
    }
}
