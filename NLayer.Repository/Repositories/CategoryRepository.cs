using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Repository.SqlServer;

namespace NLayer.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Category> GetSingleCategoryByIdWithProductAsync(int categodyId)
        {
            return await _dbContext.Categories.Include(x =>x.Products).Where(x => x.Id == categodyId).SingleOrDefaultAsync();
        }
    }
}