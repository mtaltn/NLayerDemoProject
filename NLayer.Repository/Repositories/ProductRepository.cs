﻿using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Repository.Entityframework.Contexts.AppDbContexts;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Product>> GetProductWithCategoryAsync()
        {
            /*
             * Eager Loading done
             * Lazy Loading maybe later
             */
            return await _dbContext.Products.Include(p => p.Category).ToListAsync();
        }
    }
}
