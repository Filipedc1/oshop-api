using AutoMapper;
using ShopApi.Data.Models;
using ShopApi.Core.Dto;

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
                .ForMember(x => x.ShoppingCartId, opt => opt.MapFrom(x => x.ShoppingCart.ShoppingCartId))
                .ReverseMap();

            CreateMap<ShippingDetail, ShippingDetailDto>()
                .ReverseMap();

            CreateMap<Order, OrderDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(x => x.User.UserName))
                .ReverseMap();
        }
    }
}
