﻿using AutoMapper;
using NLayer.Core.DTOs.CreateDTOs;
using NLayer.Core.DTOs.EntityDTOs;
using NLayer.Core.DTOs.UpdateDTOs;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile() 
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<ProductCreateDto, Product>().ReverseMap();
            CreateMap<Product,ProductWithCategoryDto>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryUpdateDto, Product>();
            CreateMap<Category, CategoryWithProductsDto>();

            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
        }
    }
}
