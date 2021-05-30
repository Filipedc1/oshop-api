using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Core.Dto;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IShoppingCartService cartService, IProductService productService, IMapper mapper)
        {
            _cartService = cartService;
            _productService = productService;
            _mapper = mapper;
        }

        // POST  /shoppingcart
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateCartAsync(ShoppingCartDto cartDto)
        {
            if (cartDto is null) return NotFound();

            int cartId = 0;

            try
            {
                var cart = new ShoppingCart
                {
                    DateCreatedUtc = DateTime.Parse(cartDto.DateCreatedUtc).ToUniversalTime()
                };

                cartId = await _cartService.CreateCartAsync(cart);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(new { cartId });
        }


        // GET  /shoppingcart/createcart
        [HttpGet("createcart")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> CreateCartAsync()
        {
            try
            {
                var cart = new ShoppingCart
                {
                    DateCreatedUtc = DateTime.UtcNow
                };

                int cartId = await _cartService.CreateCartAsync(cart);

                var dto = new ShoppingCartDto
                {
                    ShoppingCartId = cartId,
                    DateCreatedUtc = cart.DateCreatedUtc.ToString("MM/dd/yyyy HH:mm:ss"),
                };

                return Ok(dto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET  /shoppingcart/12
        [HttpGet("{cartId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetCartAsync(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);

            if (cart == null)
                return NotFound();

            var dto = new ShoppingCartDto
            {
                ShoppingCartId = cart.ShoppingCartId,
                DateCreatedUtc = cart.DateCreatedUtc.ToString("MM/dd/yyyy HH:mm:ss"),
                ShoppingCartItems = _mapper.Map<IEnumerable<ShoppingCartItemDto>>(cart.ShoppingCartItems)
            };

            return Ok(dto);
        }

        [HttpGet("getitemfromcart/{cartId}/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetItemFromCartAsync(int cartId, int productId)
        {
            var dto = new ShoppingCartItemDto();

            try
            {
                var item = await _cartService.GetItemFromCartAsync(cartId, productId);

                if (item == null)
                    return Ok(null);

                dto = _mapper.Map<ShoppingCartItemDto>(item);
            }
            catch (Exception e)
            {

            }

            return Ok(dto);
        }

        // POST  /shoppingcart
        [HttpPost("additemtocart/{cartId}")]
        [AllowAnonymous]
        public async Task<ActionResult> AddItemToCartAsync(int cartId, ShoppingCartItemDto cartItemDto)
        {
            if (cartItemDto is null) return NotFound();

            //var prod = await _productService.GetProductByIdAsync(cartItemDto.ProductId);

            var item = new ShoppingCartItem
            {
                ProductId = cartItemDto.ProductId,
                ProductName = cartItemDto.ProductName,
                Price = cartItemDto.Price,
                Quantity = cartItemDto.Quantity,
                ImageUrl = cartItemDto.ImageUrl,
                CategoryId = cartItemDto.CategoryId,
                ShoppingCart = await _cartService.GetCartByIdAsync(cartId)
            };

            await _cartService.AddItemToCartAsync(item);

            return Ok();
        }

        [HttpPut("updateitemquantity")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateItemQuantityAsync(ShoppingCartItemDto cartDto)
        {
            if (cartDto is null) return NotFound();

            bool success = await _cartService.UpdateItemQuantityAsync(cartDto.ShoppingCartId, cartDto.ProductId, cartDto.Quantity);
            if (!success)
                return BadRequest();

            return Ok();
        }

        [HttpGet("clearcart/{cartId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> ClearCartAsync(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);

            if (cart is null)
                return NotFound();

            await _cartService.ClearCartAsync(cart);

            return Ok();
        }
    }
}