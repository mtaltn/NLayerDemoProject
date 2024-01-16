using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.DTOs.CreateDTOs;
using NLayer.Core.DTOs.EntityDTOs;
using NLayer.Core.DTOs.ResponseDTOs;
using NLayer.Core.DTOs.UpdateDTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System.Linq.Expressions;

namespace NLayer.Service.Services
{
    public class ProductServiceWithDto : ServiceWithDto<Product, ProductDto>, IProductServiceWithDto
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceWithDto(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto)
        {
            var newEntity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = _mapper.Map<ProductDto>(newEntity);
            return CustomResponseDto<ProductDto>.Success(StatusCodes.Status201Created, newDto);
        }

        public async Task<CustomResponseDto<IEnumerable<ProductCreateDto>>> AddRangeAsync(IEnumerable<ProductCreateDto> dtos)
        {
            var newEntitis = _mapper.Map<IEnumerable<Product>>(dtos);
            await _productRepository.AddRangeAsync(newEntitis);
            await _unitOfWork.CommitAsync();
            var newDto = _mapper.Map<IEnumerable<ProductCreateDto>>(newEntitis);
            return CustomResponseDto<IEnumerable<ProductCreateDto>>.Success(StatusCodes.Status201Created, newDto);
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory()
        {
            var products = await _productRepository.GetProductWithCategoryAsync();
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        
    }
}
