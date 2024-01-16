using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs.CreateDTOs;
using NLayer.Core.DTOs.UpdateDTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/v2/[controller]")]
    public class ProductsWithDtoController : CustomBaseController
    {
        private readonly IProductServiceWithDto _productServiceWithDto;

        public ProductsWithDtoController(IProductServiceWithDto productServiceWithDto)
        {
            _productServiceWithDto = productServiceWithDto;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productServiceWithDto.GetProductWithCategory());
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            return CreateActionResult(await _productServiceWithDto.GetAllAsync());            
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _productServiceWithDto.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto productCreateDto)
        {
            return CreateActionResult(await _productServiceWithDto.AddAsync(productCreateDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            return CreateActionResult(await _productServiceWithDto.UpdateAsync(productUpdateDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return CreateActionResult(await _productServiceWithDto.RemoveAsync(id));            
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> SaveAll(IEnumerable<ProductCreateDto> productCreateDto)
        {
            return CreateActionResult(await _productServiceWithDto.AddRangeAsync(productCreateDto));            
        }
        
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveAll(List<int> ids)
        {
            return CreateActionResult(await _productServiceWithDto.RemoveRangeAsync(ids));            
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Any(int id)
        {
            return CreateActionResult(await _productServiceWithDto.AnyAsync(x => x.Id == id));            
        }


    }
}
