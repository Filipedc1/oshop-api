using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.DTOs;

namespace ShopApi
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap(); 

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.Category.CategoryId))
                .ReverseMap();

            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ReverseMap();

            CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
                //.ForMember(x => x.ShoppingCartId, opt => opt.MapFrom(x => x.ShoppingCart.ShoppingCartId))
                .ReverseMap();
        }
    }
}
