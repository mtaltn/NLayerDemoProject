﻿using AutoMapper;
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
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<CategoryUpdateDto, Product>();
        }
    }
}