using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET  /products
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();

            if (products == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        // GET  /products/12
        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // POST  /products
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddProductAsync(ProductDto productDto)
        {
            if (productDto is null) return NotFound();

            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _productService.AddProductAsync(product);
            }
            catch(Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}