using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System.Reflection;

namespace NLayer.Repository.Entityframework.Contexts.AppDbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public override int SaveChanges()
        {
            UpdateChangeTracker();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTracker();
            return base.SaveChangesAsync(cancellationToken);
        }
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

        public void UpdateChangeTracker()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entityReference)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            {
                                Entry(entityReference).Property(x => x.UpdateDate).IsModified = false;
                                entityReference.CreateDate = DateTime.Now;
                                break;
                            }

                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreateDate).IsModified = false;
                                entityReference.UpdateDate = DateTime.Now;
                                break;
                            }

                    }
                }
            }
        }
    }
}
