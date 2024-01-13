using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    internal class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasData(new ProductFeature
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 200,
                Width = 200,
                ProductId = 1,
            },
            new ProductFeature
            {
                Id = 2,
                Color = "Mavi",
                Height = 250,
                Width = 100,
                ProductId = 2,
            },
            new ProductFeature
            {
                Id = 3,
                Color = "Turuncu",
                Height = 100,
                Width = 270,
                ProductId = 3,
            }
            );
        }
    }
}
