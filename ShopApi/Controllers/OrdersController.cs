using System;
using System.Collections.Generic;
using System.Globalization;
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

        // GET  /orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            var orders = await _orderService.GetOrdersAsync();

            if (orders == null)
                return NotFound();

            var ordersDto = orders.Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                Total = x.Total,
                DatePlaced = x.PlacedUtc.ToLocalTime().ToString("g", CultureInfo.CreateSpecificCulture("en-us")),
                Username = x.User?.UserName,
                Shipping = _mapper.Map<ShippingDetailDto>(x.ShippingDetail),
                Items = x.OrderDetails.Select(od => new ShoppingCartItemDto
                {
                    ProductId = od.ProductId,
                    ProductName = od.ProductName,
                    Quantity = od.ProductQuantity,
                    Price = od.ProductPrice
                })
            });

            return Ok(ordersDto);
        }

        // GET  /orders/username
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();

            var orders = await _orderService.GetOrdersByUserIdAsync(user.Id);

            if (orders == null)
                return NotFound();

            var ordersDto = orders.Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                Total = x.Total,
                DatePlaced = x.PlacedUtc.ToLocalTime().ToString("g", CultureInfo.CreateSpecificCulture("en-us")),
                Username = x.User?.UserName,
                Shipping = _mapper.Map<ShippingDetailDto>(x.ShippingDetail),
                Items = x.OrderDetails.Select(od => new ShoppingCartItemDto
                {
                    ProductId = od.ProductId,
                    ProductName = od.ProductName,
                    Quantity = od.ProductQuantity,
                    Price = od.ProductPrice
                })
            });

            return Ok(ordersDto);
        }

        // POST  /orders
        [HttpPost]
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