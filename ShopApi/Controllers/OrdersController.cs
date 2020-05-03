using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using ShopApi.DTOs;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        private readonly UserManager<AppUser> _userManager;

        private readonly IMapper _mapper;

        public OrdersController(IShoppingCartService cartService, IProductService productService, IOrderService orderService,
                                UserManager<AppUser> userManager, IMapper mapper)
        {
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
            _userManager = userManager;
            _mapper = mapper;
        }

        // POST  /orders
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            if (orderDto == null) 
                return BadRequest();

            try
            {
                var user = await _userManager.FindByNameAsync(orderDto.Username);
                if (user == null) 
                    return BadRequest();

                var order = new Order
                {
                    ShippingDetail = _mapper.Map<ShippingDetail>(orderDto.Shipping),
                    PlacedUtc = DateTime.Parse(orderDto.DatePlaced).ToUniversalTime(),
                    Total = GetOrderTotal(orderDto.Items),
                    User = user
                };

                int cartId = orderDto.Items.FirstOrDefault().ShoppingCartId;

                var orderDetails = orderDto.Items.Select(x => new OrderDetail
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductQuantity = x.Quantity,
                    ProductPrice = x.Price,
                    Order = order
                });

                await _orderService.AddOrderAsync(order, orderDetails);
                await _cartService.ClearCartAsync(cartId);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        private decimal GetOrderTotal(IEnumerable<ShoppingCartItemDto> items)
        {
            return items.Sum(x => x.Price * x.Quantity);
        }
    }
}