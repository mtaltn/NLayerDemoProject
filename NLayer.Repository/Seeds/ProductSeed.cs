using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;

namespace NLayer.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(new Product
            {
                Id = 1,
                CategoryId = 1,
                Name = "Dolma Kalem",
                Price = 100,
                Stock = 20,
                CreateDate = DateTime.Now
            },
            new Product
            {
                Id = 2,
                CategoryId = 1,
                Name = "Tükenmez Kalem",
                Price = 500,
                Stock = 50,
                CreateDate = DateTime.Now
            },
            new Product
            {
                Id = 3,
                CategoryId = 1,
                Name = "Tükenmez Kalem",
                Price = 500,
                Stock = 50,
                CreateDate = DateTime.Now
            },
            new Product
            {
                Id = 4,
                CategoryId = 2,
                Name = "Nutuk",
                Price = 1881,
                Stock = 1938,
                CreateDate = DateTime.Now
            },
            new Product
            {
                Id = 5,
                CategoryId = 2,
                Name = "Şu Çılgın Türkler",
                Price = 1071,
                Stock = 9999,
                CreateDate = DateTime.Now
            }
            );
        }
    }
}
