using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using ShopApi.DTOs;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IShoppingCartService cartService, IMapper mapper)
        {
            _cartService = cartService;
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
                    DateCreatedUtc = DateTime.Parse(cartDto.DateCreated).ToUniversalTime()
                };

                cartId = await _cartService.CreateCartAsync(cart);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(new { cartId });
        }

        // GET  /shoppingcart/12
        [HttpGet("{cartId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetCartAsync(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);

            if (cart == null)
                return NotFound();

            var dto = _mapper.Map<ShoppingCartDto>(cart);

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
        public async Task<ActionResult> AddItemToCartAsync(int cartId, ShoppingCartItemDto cartDto)
        {
            if (cartDto is null) return NotFound();

            var item = new ShoppingCartItem
            {
                ProductId = cartDto.ProductId,
                Quantity = cartDto.Quantity,
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

            bool success = await _cartService.UpdateItemQuantityAsync(cartDto.ProductId);
            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}