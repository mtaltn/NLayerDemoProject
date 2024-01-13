using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Repository.Configurations;
using System.Reflection;

namespace NLayer.Repository.SqlServer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ProductFeature> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            /*
             *If you want to enable only one of the IEntityTypeConfiguration contents, but not all of them, you can use this configuration.
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            */

            /*
             * If you want, you can add seed data this way as well.
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature() {
                Id = 1,
                Color = "Kırmızı",
                Height = 200,
                Width = 200,
                ProductId = 1,
            });
            */
            base.OnModelCreating(modelBuilder);
        }
    }
}
