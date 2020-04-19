﻿using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.DTOs;

namespace ShopApi
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Category, CategoryDto>();


            //CreateMap<Product, ProductDto>()
            //    .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category.CategoryId))
            //    .ReverseMap();

            //CreateMap<ProductDto, Product>();
        }
    }
}
