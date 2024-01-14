using NLayer.Core.DTOs.EntityDTOs;
using NLayer.Core.DTOs.ResponseDTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        public Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductAsync(int categodyId);
    }
}
