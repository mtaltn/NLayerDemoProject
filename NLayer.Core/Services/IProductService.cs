using NLayer.Core.DTOs.EntityDTOs;
using NLayer.Core.DTOs.ResponseDTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory();
    }
}
