using NLayer.Core.DTOs.CreateDTOs;
using NLayer.Core.DTOs.EntityDTOs;
using NLayer.Core.DTOs.ResponseDTOs;
using NLayer.Core.DTOs.UpdateDTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface IProductServiceWithDto : IServiceWithDto<Product, ProductDto>
    {
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory();
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto);
        Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto);
        Task<CustomResponseDto<IEnumerable<ProductCreateDto>>> AddRangeAsync(IEnumerable<ProductCreateDto> dtos);
    }
}
