using AutoMapper;
using NLayer.Core.DTOs.EntityDTOs;
using NLayer.Core.DTOs.ResponseDTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.UnitOfWorks;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;


namespace NLayer.Caching.Redis.Repositories
{
    public class ProductServiceWithCacheDecorator : IProductService
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _repository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServiceWithCacheDecorator(RedisService redisService, IProductRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(1);
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            if (await _cacheRepository.KeyExistsAsync(productKey))
                await _cacheRepository.HashSetAsync(productKey, entity.Id, JsonSerializer.Serialize(entity));
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await Task.WhenAll(entities.Select(async entity =>
            {
                if (await _cacheRepository.KeyExistsAsync(productKey))
                    await _cacheRepository.HashSetAsync(productKey, entity.Id, JsonSerializer.Serialize(entity));
            }));

            return entities;

        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);

            if (cacheProducts.Any())
            {
                return cacheProducts
                    .Select(item => JsonSerializer.Deserialize<Product>(item.Value))
                    .Any(expression.Compile());
            }

            var productsFromDb = _repository.GetAll();
            return productsFromDb.Any(expression.Compile());
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);

            return cacheProducts
                .Select(item => JsonSerializer.Deserialize<Product>(item.Value))
                .ToList();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : new Product();
            }
            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory()
        {
            var cachedProducts = await _cacheRepository.HashGetAllAsync(productKey);
            if (cachedProducts.Any())
            {
                var products = cachedProducts
                    .Select(item => JsonSerializer.Deserialize<Product>(item.Value))
                    .ToList();
                var productWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
                return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productWithCategoryDto);
            }

            var productsFromDb = await _repository.GetProductWithCategoryAsync();
            foreach (var product in productsFromDb)
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            }
            var productWithCategoryDtoFromDb = _mapper.Map<List<ProductWithCategoryDto>>(productsFromDb);

            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productWithCategoryDtoFromDb);
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();

            if (await _cacheRepository.KeyExistsAsync(productKey))
                await _cacheRepository.HashDeleteAsync(productKey, entity.Id);
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            var entityIds = entities.Select(e => (RedisValue)e.Id).ToArray();
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();

            if (await _cacheRepository.KeyExistsAsync(productKey))
                await _cacheRepository.HashDeleteAsync(productKey, entityIds);
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            if (await _cacheRepository.KeyExistsAsync(productKey))
                await _cacheRepository.HashSetAsync(productKey, entity.Id, JsonSerializer.Serialize(entity));
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            var cachedProducts = _cacheRepository.HashGetAll(productKey)
                .Select(item => JsonSerializer.Deserialize<Product>(item.Value))
                .AsQueryable();
            if (cachedProducts.Any())
            {
                return cachedProducts.Where(expression);
            }
            return Enumerable.Empty<Product>().AsQueryable();
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var response = await _repository.GetProductWithCategoryAsync();
            if (response is not null)
            {
                var productWithCategoryList = response;
                var products = _mapper.Map<List<Product>>(productWithCategoryList);                
                foreach (var product in products)
                {
                    await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
                }
                return products;
            }
            else
            {
                return new List<Product>(); 
            }
        }
    }
}
